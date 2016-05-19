using Newtonsoft.Json;
using WhatsappGroups.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using WhatsappGroups.Data.Contexts;

namespace WhatsappGroups.Business.Core
{
    public static class SeedData
    {
        //private static List<RechargeType> rechargeTypes = new List<RechargeType>()
        //{
        //    new RechargeType() { Code = "TUP", ShortName = "Top Ups", Name = "Top-up Recharge" },
        //    new RechargeType() { Code = "FTT", ShortName = "Full Talk Time", Name = "Full Talk-time Recharge" },
        //    new RechargeType() { Code = "2G", ShortName = "2G Data", Name = "2G Data Recharge" },
        //    new RechargeType() { Code = "3G", ShortName = "3G Data", Name = "3G/4G Data Recharge" },
        //    new RechargeType() { Code = "SMS", ShortName = "SMS Packs", Name = "SMS Pack Recharge" },
        //    new RechargeType() { Code = "LSC", ShortName = "Local/STD/ISD", Name = "Local/STD/ISD Call Recharge" },
        //    new RechargeType() { Code = "RMG", ShortName = "National/International", Name = "National/International Roaming Recharge" },
        //    new RechargeType() { Code = "OTR",ShortName = "Other",  Name = "Other Recharge" }
        //};

        //private static List<Circle> circles = new List<Circle>()
        //{
        //    new Circle() { Code = "1", Name = "Delhi/NCR" },
        //    new Circle() { Code = "2", Name = "Mumbai" },
        //    new Circle() { Code = "3", Name = "Kolkata" },
        //    new Circle() { Code = "4", Name = "Maharashtra" },
        //    new Circle() { Code = "5", Name = "Andhra Pradesh" },
        //    new Circle() { Code = "6", Name = "Tamil Nadu" },
        //    new Circle() { Code = "7", Name = "Karnataka" },
        //    new Circle() { Code = "8", Name = "Gujarat" },
        //    new Circle() { Code = "9", Name = "Uttar Pradesh (E)" },
        //    new Circle() { Code = "10", Name = "Madhya Pradesh" },
        //    new Circle() { Code = "11", Name = "Uttar Pradesh (W)" },
        //    new Circle() { Code = "12", Name = "West Bengal" },
        //    new Circle() { Code = "13", Name = "Rajasthan" },
        //    new Circle() { Code = "14", Name = "Kerala" },
        //    new Circle() { Code = "15", Name = "Punjab" },
        //    new Circle() { Code = "16", Name = "Haryana" },
        //    new Circle() { Code = "17", Name = "Bihar & Jharkhand" },
        //    new Circle() { Code = "18", Name = "Orissa" },
        //    new Circle() { Code = "19", Name = "Assam" },
        //    new Circle() { Code = "20", Name = "North East" },
        //    new Circle() { Code = "21", Name = "Himachal Pradesh" },
        //    new Circle() { Code = "22", Name = "Jammu & Kashmir" },
        //    new Circle() { Code = "23", Name = "Chennai" }
        //};

        //private static List<Operator> operators = new List<Operator>()
        //{
        //    new Operator() { Code = "1", Name = "Aircel" },
        //    new Operator() { Code = "3", Name = "BSNL" },
        //    new Operator() { Code = "5", Name = "Videocon" },
        //    new Operator() { Code = "6", Name = "MTNL Mumbai" },
        //    new Operator() { Code = "8", Name = "Idea" },
        //    new Operator() { Code = "9", Name = "Loop" },
        //    new Operator() { Code = "10", Name = "MTS" },
        //    new Operator() { Code = "12", Name = "Reliance CDMA" },
        //    new Operator() { Code = "13", Name = "Reliance GSM" },
        //    new Operator() { Code = "17", Name = "TATA Docomo GSM" },
        //    new Operator() { Code = "18", Name = "TATA Docomo CDMA" },
        //    new Operator() { Code = "19", Name = "Uninor" },
        //    new Operator() { Code = "20", Name = "MTNL Delhi" },
        //    new Operator() { Code = "22", Name = "Vodafone" },
        //    new Operator() { Code = "28", Name = "Airtel" },
        //    new Operator() { Code = "30", Name = "Virgin GSM" },
        //    new Operator() { Code = "32", Name = "Virgin CDMA" },
        //    new Operator() { Code = "33", Name = "T24" }
        //};
        
        //public static Task Ensure()
        //{
        //    return Task.Factory.StartNew(() =>
        //    {
        //        var db = new WhatsappGroupsDataContext();
        //        db.Operators.AddRange(operators);
        //        db.Circles.AddRange(circles);
        //        db.RechargeTypes.AddRange(rechargeTypes);

        //        db.SaveChanges();

        //        db.Circles.ToList().ForEach(c => db.Operators.ToList().ForEach(o => db.OperatorCircles.Add(new OperatorCircle() { CircleId = c.Id, OperatorId = o.Id })));
        //        db.RechargeTypes.ToList().ForEach(r => db.Circles.ToList().ForEach(c => db.CircleRechargeTypes.Add(new CircleRechargeType() { CircleId = c.Id, RechargeTypeId = r.Id })));

        //        db.SaveChanges();
                
        //    });
        //}

        //public static Task Feed()
        //{
        //    return Task.Factory.StartNew(() => 
        //    {
        //        using (var db = new WhatsappGroupsDataContext())
        //        {
        //            using (var client = new HttpClient() { BaseAddress = new Uri("https://joloapi.com/") })
        //            {
        //                db.Operators.ToList().ForEach(o => db.Circles.ToList().ForEach(c => db.RechargeTypes.ToList().ForEach(r =>
        //                {
        //                    try
        //                    {
        //                        var res = client.GetAsync("api/findplan.php?type=json&userid=" + Configs.Username + "&key=" + Configs.Key + "&opt=" + o.Code + "&cir=" + c.Code + "&typ=" + r.Code).Result;
        //                        res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //                        if (res.IsSuccessStatusCode)
        //                        {
        //                            var cards = JsonConvert.DeserializeObject<List<RechargeCard>>(res.Content.ReadAsStringAsync().Result);
        //                            if(!db.RechargeCards.Any(rc => rc.RechargeTypeId == r.Id && rc.OperatorId == o.Id && rc.CircleId == c.Id ))
        //                                cards.ForEach(card => 
        //                                {
        //                                    card.OperatorId = o.Id;
        //                                    card.CircleId = c.Id;
        //                                    card.RechargeTypeId = r.Id;
        //                                    db.RechargeCards.Add(card);
        //                                });

        //                        }
        //                    }
        //                    catch (Exception e)
        //                    {

        //                    }
        //                })));

        //                db.SaveChanges();
        //            }
        //        }
        //    });
        //}
    }
}
