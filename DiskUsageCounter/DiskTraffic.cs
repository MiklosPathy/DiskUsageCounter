using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Timers;
using System.ComponentModel;

namespace DiskUsageCounter
{
    public class DiskTraffic : INotifyPropertyChanged
    {
        public Int64 Reads { get; set; }
        public Int64 Writes { get; set; }

        private PerformanceCounter diskReadCounter = new PerformanceCounter();
        private PerformanceCounter diskWriteCounter = new PerformanceCounter();
        private Timer mytimer = new Timer();

        private void TimerElapsed(object source, ElapsedEventArgs e)
        {
            Reads = Reads + (Int64)diskReadCounter.NextValue();
            Writes = Writes + (Int64)diskWriteCounter.NextValue();

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Reads"));
                PropertyChanged(this, new PropertyChangedEventArgs("Writes"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DiskTraffic()
        {
            diskReadCounter.CategoryName = "PhysicalDisk";
            diskReadCounter.CounterName = "Disk Read Bytes/Sec";
            diskReadCounter.InstanceName = "_Total";
            diskWriteCounter.CategoryName = "PhysicalDisk";
            diskWriteCounter.CounterName = "Disk Write Bytes/Sec";
            diskWriteCounter.InstanceName = "_Total";

            mytimer.Elapsed += TimerElapsed;
            mytimer.Interval = 1000;
            mytimer.Enabled = true;
        }
    }
}
