using System;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels
{
	public class ElementVM
	{
        public IEnumerable<Client> Clients { get; set; } = new List<Client>();
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
    }
}

