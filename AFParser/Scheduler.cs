using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace AFParser
{
    class Scheduler
    {
        private readonly ConcurrentDictionary<Action, ScheduledTask> _scheduledTasks = new ConcurrentDictionary<Action, ScheduledTask>();

        public void Execute(int timeoutMs,Action action)
        {
            var task = new ScheduledTask(action, timeoutMs);
            task.TaskComplete += RemoveTask;
            _scheduledTasks.TryAdd(action, task);
            task.Timer.Start();
        }

        private void RemoveTask(object sender, EventArgs e)
        {
            var task = (ScheduledTask)sender;
            task.TaskComplete -= RemoveTask;
            ScheduledTask deleted;
            _scheduledTasks.TryRemove(task.Action, out deleted);
        }
    }
}
