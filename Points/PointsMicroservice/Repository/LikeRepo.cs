﻿using PointsMicroservice.Infrastructure;
using PointsMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PointsMicroservice.Repository
{
    public class LikeRepo : ILike
    {
        public static Dictionary<int, int> EmployeePoints;
        public static List<Like> Likes;
        public LikeRepo()
        {
            Likes = new List<Like>()
            {
                new Like {OfferId=1,LikeDate= new DateTime(2022, 06, 21)},
                new Like {OfferId =1, LikeDate = new DateTime(2022, 06, 21)},
                new Like { OfferId = 1, LikeDate = new DateTime(2022, 06, 22) },

                new Like { OfferId = 2, LikeDate = new DateTime(2022, 06, 21) },
                new Like { OfferId = 2, LikeDate = new DateTime(2022, 06, 22) },
                new Like { OfferId = 2, LikeDate = new DateTime(2022, 06, 21) },
                new Like { OfferId = 2, LikeDate = new DateTime(2022, 06, 21) },
                new Like { OfferId = 2, LikeDate = new DateTime(2022, 06, 22) },
                new Like { OfferId = 2, LikeDate = new DateTime(2022, 06, 21) },

                new Like {OfferId=3,LikeDate= new DateTime(2022, 06, 24)},
                new Like {OfferId =3, LikeDate = new DateTime(2022, 06, 25)},
                new Like { OfferId = 3, LikeDate = new DateTime(2022, 06, 25) },
                new Like { OfferId = 3, LikeDate = new DateTime(2022, 06, 26) },

                new Like { OfferId = 4, LikeDate = new DateTime(2022, 06, 29) },
                new Like { OfferId = 4, LikeDate = new DateTime(2022, 06, 20) },
                new Like { OfferId = 4, LikeDate = new DateTime(2022, 06, 20) },

                new Like { OfferId = 5, LikeDate = new DateTime(2022, 06, 20) },
                new Like { OfferId = 5, LikeDate = new DateTime(2022, 06, 21) },
                new Like { OfferId = 5, LikeDate = new DateTime(2022, 06, 21) },
                new Like { OfferId = 6, LikeDate = new DateTime(2022, 06, 22) },

            };
            EmployeePoints = new Dictionary<int, int>()
            {
               {2159447,34},
               {2142912,25},
               {2144009,15},
               {2159071,27},
               {2141156,18},
               {2142922,40}
            };
        }
        public int LikeInTwoDaysCount(int id, DateTime date)
        {
            int count = Likes.Where(c => c.LikeDate == date && c.OfferId == id).Count();
            return count;
        }

        public int LikeInTwoDaysCount1(int id, DateTime date)
        {
            int count1 = Likes.Where(c => c.LikeDate == date.AddDays(1) && c.OfferId == id).Count();
            return count1;
        }

        public Points PointsByEmployeeId(int employeeId)
        {
            int p;
            Points point = new Points();
            for (int i = 0; i < EmployeePoints.Count; i++)
            {

                if (EmployeePoints.ElementAt(i).Key == employeeId)
                {
                    p = EmployeePoints.ElementAt(i).Value;

                    point.TotalPoints = p;
                    point.EmployeeId = employeeId;
                    return point;
                }
            }
            return point;
        }

        public Points Refersh(int employeeId, List<OfferData> newOffers)
        {
            int id, likes_two_days, totalPoints = 0, count, count1;

            Points points = new Points();

            DateTime date;
            var employeeoffer = newOffers.Where(c => c.EmployeeId == employeeId).ToList();
            foreach (var e in employeeoffer)
            {
                id = e.OfferId;
                date = e.OpenedDate;
                count = LikeInTwoDaysCount(id, date);
                count1 = LikeInTwoDaysCount1(id, date);
                likes_two_days = count + count1;
                e.LikesInTwoDays = likes_two_days;
            }

            foreach (var e in employeeoffer)
            {
                TimeSpan engaggedDuration = e.EngagedDate - e.OpenedDate;
                if (e.LikesInTwoDays > 2)
                    totalPoints += 25;
                else if (e.LikesInTwoDays > 4)
                    totalPoints += 50;
                else if (e.Status == "Engaged" && engaggedDuration.TotalDays <= 2)
                {
                    totalPoints += 100;
                }
            }

            EmployeePoints[employeeId] += totalPoints;
            //EmployeePoints.Add(employeeId,totalPoints);

            //  return totalPoints;

            points.EmployeeId = employeeId;
            points.TotalPoints = EmployeePoints[employeeId];
            return points;

        }
    }
}
