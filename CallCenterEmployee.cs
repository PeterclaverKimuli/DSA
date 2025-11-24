using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public enum EmployeeRank { 
        Respondent = 1,
        Manager = 2,
        Director = 3
    }

    public class Call {
        public EmployeeRank RequiredRank { get; private set; }

        public Call(EmployeeRank requiredRank)
        {
            RequiredRank = requiredRank;
        }
    }

    public abstract class CallCenterEmployee
    {
        public string Name { get; private set; }
        public EmployeeRank Rank { get; protected set; }
        public bool isAvailable { get; set; }

        public CallCenterEmployee(string name)
        {
            Name = name;
            isAvailable = true;
        }

        public abstract bool CanHandle(Call call);
    }

    public class Respondent : CallCenterEmployee {
        public Respondent(string name) : base(name) {
            Rank = EmployeeRank.Respondent;
        }

        public override bool CanHandle(Call call)
        {
            return Rank <= call.RequiredRank;
        }
    }

    public class Manager : CallCenterEmployee {
        public Manager(string name) : base(name)
        {
            Rank = EmployeeRank.Manager;
        }

        public override bool CanHandle(Call call)
        {
            return Rank <= call.RequiredRank;
        }
    }

    public class Director : CallCenterEmployee {
        public Director(string name) : base(name)
        {
            Rank = EmployeeRank.Director;
        }

        public override bool CanHandle(Call call)
        {
            return true; // Directors can handle all calls
        }
    }

    public class CallHandler 
    {
        private Queue<Respondent> respondents = new Queue<Respondent>();
        private Queue<Manager> managers = new Queue<Manager>();
        private Queue<Director> directors = new Queue<Director>();

        public void AddEmployee(CallCenterEmployee employee) {
            if (employee is Respondent r)
            {
                respondents.Enqueue(r);
            }
            else if (employee is Manager m)
                managers.Enqueue(m);
            else if (employee is Director d)
                directors.Enqueue(d);
            else
                throw new ArgumentException("Unknown employee type");
        }

        public void DispatchCall(Call call) { 
            CallCenterEmployee handler = GetCallHandler(call);

            if (handler is not null) {
                handler.isAvailable = false; // Mark the employee as busy
                Console.WriteLine($"Call is being handled by {handler.GetType().Name} {handler.Name}");
            }
            else 
                Console.WriteLine("All employees are currently busy. Please wait...");
        }

        private CallCenterEmployee GetCallHandler(Call call) {
            if (call.RequiredRank <= EmployeeRank.Respondent) {
                foreach (var r in respondents) { 
                    if(r.isAvailable) return r;
                }
            }

            if (call.RequiredRank <= EmployeeRank.Manager)
            {
                foreach(var m in managers)
                    if(m.isAvailable) return m;
            }

            foreach(var d in directors)
                if(d.isAvailable) return d;

            return null; // No available employee found
        }
    }
}
