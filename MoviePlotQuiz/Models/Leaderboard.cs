//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoviePlotQuiz.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Leaderboard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string Genre { get; set; }
        public Nullable<int> Questions { get; set; }
        public Nullable<double> Correct { get; set; }
        public Nullable<double> Percentage { get; set; }
        public Nullable<double> Score { get; set; }
    }
}
