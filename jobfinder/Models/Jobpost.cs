using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;

namespace jobfinder.Models
{
    public class Jobpost
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public DateTime posted_date { get; set; }

        [Required]
        [StringLength(255)]
        public string location { get; set; }
        [Required]
        [StringLength(40)]
        public string company_name { get; set; }

        [Required]
        public int vacancy { get; set; }

        [Required]
        [StringLength(255)]
        public string job_nature { get; set; }

        [Required]
        public decimal salary { get; set; }

        [Required]
        public DateTime application_date { get; set; }

        public string job_description { get; set; }

        [StringLength(255)]
        public string required_skills { get; set; }

        [StringLength(255)]
        public string experience { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }

    }
    public class JobApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int jobpost_id { get; set; }

        [Required]
        [StringLength(255)]
        public string jobseeker_name { get; set; }

        [Required]
        [StringLength(255)]
        public string jobseeker_email { get; set; }

        public string cover_letter { get; set; }

        [StringLength(255)]
        public string resume_file_path { get; set; }

        [Required]
        public DateTime date_applied { get; set; }

        public virtual Jobpost Jobpost { get; set; }
    }
}
