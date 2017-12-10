﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Import
{
    [XmlType("Order")]
    public class OrderDto
    {
        [Required]
        [XmlElement("Customer")]
        public string Customer { get; set; }

        [Required]
        [XmlElement("Employee")]
        public string Employee { get; set; }

        [Required]
        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [Required]
        [XmlElement("Type")]
        public string Type { get; set; } = "ForHere";

        [Required]
        public OrderItemDto[] Items { get; set; } = new OrderItemDto[0];
    }
}