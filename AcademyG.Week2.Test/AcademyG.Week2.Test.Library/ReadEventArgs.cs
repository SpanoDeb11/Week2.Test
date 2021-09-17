using System;

namespace AcademyG.Week2.Test.Library
{
    public class ReadEventArgs : EventArgs
    {
        public Good Item { get; set; }
        public Guid WarehouseId { get; set; }
    }
}