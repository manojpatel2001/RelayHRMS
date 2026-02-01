using HRMS_Core.Master.Scheme;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmailService
{
    public static class Helper
    {
        public static string GenerateStarRating(int rating)
        {
            string filledStar = "<span class=\"star filled\">★</span>";
            string emptyStar = "<span class=\"star\">★</span>";
            StringBuilder starsHtml = new StringBuilder();
            for (int i = 1; i <= 5; i++)
            {
                starsHtml.Append(i <= rating ? filledStar : emptyStar);
            }
            return starsHtml.ToString();
        }

       
    }

}
