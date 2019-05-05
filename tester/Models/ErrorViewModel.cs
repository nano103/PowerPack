using System;

namespace tester.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class TestObject
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string about { get; set; }
    }
}