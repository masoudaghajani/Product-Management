using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public interface IEntity
    {
    }

    public abstract class BaseEntity<TKey> : IEntity
    {
        [Key]


        public TKey Id { get; set; }

        //public int CreatorUserId { get; set; }
        //public DateTime CreatedDate { get; set; } = DateTime.Now;
        //public int? UpdaterUserId { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        //public bool IsDeleted { get; set; } = false;
    }

    public abstract class BaseEntity : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }



}
