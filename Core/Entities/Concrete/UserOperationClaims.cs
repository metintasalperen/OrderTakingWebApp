using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Concrete
{
    public class UserOperationClaims : IEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("OperationClaims")]
        public int OperationClaimId { get; set; }
        public OperationClaims OperationClaims { get; set; }
    }
}
