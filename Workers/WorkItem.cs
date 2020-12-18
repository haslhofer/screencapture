using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;


namespace screencapture
{
    public enum WorkItemType
    {
        Kickoff = 0,
        TimerElapsed = 1,
        ScreenShotTaken = 2
    }

//Workitem for the main Queue
    public class WorkItem
    {
        public WorkItemType TypeOfWorkItem {get;set;}
        public object WorkItemContext {get;set;}
        public long CreatedTimeTick {get;set;} 

        public static WorkItem GetGenericWorkItem(WorkItemType TypeOfWorkItem)
        {
            WorkItem w = new WorkItem();
            w.TypeOfWorkItem = TypeOfWorkItem;
            w.CreatedTimeTick = DateTime.Now.Ticks;
            return w;
        }

        public WorkItem GetClone()
        {
            WorkItem clone = new WorkItem();
            clone.CreatedTimeTick = this.CreatedTimeTick;
            clone.TypeOfWorkItem = this.TypeOfWorkItem;
            clone.WorkItemContext = this.WorkItemContext;
            return clone;
        }
    }

    
}
