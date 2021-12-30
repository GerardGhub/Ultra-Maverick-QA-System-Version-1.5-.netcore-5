using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerQAAPICORE.Identity;

namespace TaskManagerQAAPICORE.Models
{
  public class TaskStatusDetail
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TaskStatusDetailID { get; set; }
    public int TaskID { get; set; }
    public int TaskStatusID { get; set; }
    public string UserID { get; set; }
    public string Description { get; set; }
    public DateTime StatusUpdationDateTime { get; set; }
    [NotMapped]
    public string StatusUpdationDateTimeString { get; set; }

    [ForeignKey("TaskStatusID")]
    public virtual TaskStatusModel TaskStatus { get; set; }

    [ForeignKey("UserID")]
    public virtual ApplicationUser User { get; set; }

  }
}
