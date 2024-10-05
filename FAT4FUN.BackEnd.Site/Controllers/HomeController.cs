using FAT4FUN.BackEnd.Site.Models.Dtos;
using FAT4FUN.BackEnd.Site.Models.Repositories;
using FAT4FUN.BackEnd.Site.Models.Services;
using FAT4FUN.BackEnd.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAT4FUN.BackEnd.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly OrderService _orderService;

        public HomeController()
        {
            _orderService = new OrderService();
        }


        [Authorize]
        public ActionResult Index()
        {
            // 取得所有訂單資料
            List<OrderDto> dto = new OrderRepository().Get();

            // 將 DTO 轉換為 VM
            List<OrderVm> vm = WebApiApplication._mapper.Map<List<OrderVm>>(dto);

            CalculateOrderStatistics(vm);

            return View(vm);

        }

        /// <summary>
        /// 計算訂單統計數據
        /// </summary>
        /// <param name="orders"></param>
        private void CalculateOrderStatistics(List<OrderVm> orders)
        {
            // 計算當月已完成訂單的總收入
            ViewBag.TotalCompletedOrderAmount = CalculateMonthlyCompletedOrderAmount(orders);

            // 計算當年度已完成訂單的總收入
            ViewBag.TotalAnnualCompletedOrderAmount = CalculateAnnualCompletedOrderAmount(orders);

            // 計算尚未處理的訂單筆數
            ViewBag.PendingOrdersCount = GetPendingOrdersCount(orders);

            // 計算正在退貨的案件筆數
            ViewBag.ReturningOrdersCount = GetReturningOrdersCount(orders);

            // 設定年度銷售目標並計算達成率
            const decimal annualSalesTarget = 5000000; // 500萬銷售目標
            decimal totalAnnualSales = ViewBag.TotalAnnualCompletedOrderAmount;
            ViewBag.SalesAchievementRate = CalculateSalesAchievementRate(totalAnnualSales, annualSalesTarget);

            // 獲取每個月已完成訂單的總收入
            // 獲取每個月已完成訂單的總收入
            List<int> monthlyAmounts = GetMonthlyCompletedOrderAmounts(orders);
            ViewBag.MonthlyCompletedOrderAmountsJson = Newtonsoft.Json.JsonConvert.SerializeObject(monthlyAmounts);
        }


        /// <summary>
        /// 每個月已完成訂單的總收入
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        private List<int> GetMonthlyCompletedOrderAmounts(List<OrderVm> orders)
        {
            var currentYear = DateTime.Now.Year;

            var monthlyAmounts = new List<int>();

            for (int month = 1; month <= 12; month++)
            {
                var totalCompletedOrderAmount = orders
                    .Where(order => order.Status == 3 &&
                                    order.ModifyDate.Year == currentYear &&
                                    order.ModifyDate.Month == month)
                    .Sum(order => order.TotalAmount);

                monthlyAmounts.Add(totalCompletedOrderAmount); // 每個月的總收入，若為0則加0
            }

            return monthlyAmounts;
        }

        /// <summary>
        /// 當月總收入
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        private int CalculateMonthlyCompletedOrderAmount(List<OrderVm> orders)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            int totalCompletedOrderAmount = orders
                .Where(order => order.Status == 3 &&
                                order.CreateDate.Year == currentYear &&
                                order.CreateDate.Month == currentMonth)
                .Sum(order => order.TotalAmount); // 假設 TotalAmount 是訂單的金額欄位

            return totalCompletedOrderAmount;
        }

        /// <summary>
        /// 當年度總收入
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        private int CalculateAnnualCompletedOrderAmount(List<OrderVm> orders)
        {
            var currentYear = DateTime.Now.Year;

            int totalAnnualCompletedOrderAmount = orders
                .Where(order => order.Status == 3 &&
                                order.CreateDate.Year == currentYear)
                .Sum(order => order.TotalAmount);

            return totalAnnualCompletedOrderAmount;
        }

        /// <summary>
        /// 業績達成率
        /// </summary>
        /// <param name="totalAnnualSales"></param>
        /// <param name="salesTarget"></param>
        /// <returns></returns>
        private decimal CalculateSalesAchievementRate(decimal totalAnnualSales, decimal salesTarget)
        {

            if (salesTarget == 0) return 0; // 避免除以零的錯誤
            return (totalAnnualSales / salesTarget) * 100;
        }

        /// <summary>
        /// 未處理訂單
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        private int GetPendingOrdersCount(List<OrderVm> orders)
        {
            return orders
                .Where(order => order.Status == 1) // 1 代表尚未處理
                .Count();
        }

        /// <summary>
        /// 退貨中訂單
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        private int GetReturningOrdersCount(List<OrderVm> orders)
        {
            return orders
                .Where(order => order.Status == 4) // 4 代表正在退貨
                .Count();
        }


    }
}
