using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMicroservice.Models
{
    public class QuizContext: DbContext
    {

        public QuizContext(DbContextOptions<QuizContext> options):base(options)
        {

        }
        public virtual DbSet<Quiz> Quiz { get; set; }
    }
}
