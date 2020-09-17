using System;

namespace CP.API.Dto
{
    public class PhotoForDetailsDto
    {
        public int PhotoId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
    }
}