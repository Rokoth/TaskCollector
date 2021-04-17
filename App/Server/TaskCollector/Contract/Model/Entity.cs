using System;
using System.ComponentModel.DataAnnotations;

namespace TaskCollector.Contract.Model
{
    public class Entity
    {
        [Display(Name = "Идентификатор")]
        public Guid Id { get; set; }
    }
}