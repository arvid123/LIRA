using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LIRA
{
    public class Issue
    {
        public readonly DateTime creationTime = DateTime.Now;
        public IssueState state { get; private set; }
        public readonly IssueType type;
        private Issue? parent;
        private List<Issue> children = new List<Issue>();
        public readonly string title;

        public Issue(string title, IssueType type)
        {
            state = IssueState.TODO;
            this.title = title;
            this.type = type;
        }

        public void SetState(IssueState state)
        {
            if (state == IssueState.DONE)
            {
                // Check if children and grandchildren are done
                InvalidOperationException ex = new InvalidOperationException("Cannot set state to done when children/grandchildren aren't done");
                if (!areChildrenDone()) throw ex;
                foreach (var child in children)
                {
                    if (!child.areChildrenDone()) throw ex;
                }
            }

            this.state = state;
        }

        private bool areChildrenDone()
        {
            foreach (var child in children)
            {
                if (!(child.state == IssueState.DONE)) return false;
            }
            return true;
        }

        public void SetParent(Issue parent)
        {
            if (this.parent == parent) return;
            switch(this.type)
            {
                case IssueType.EPIC:
                    throw new InvalidOperationException("Epics are always orphans :(");
                case IssueType.FEATURE:
                    if (parent.type == IssueType.EPIC)
                    {
                        this.parent = parent;
                        this.parent.children.Add(this);
                        return;
                    } else
                    {
                        throw new InvalidOperationException("Features can only have Epics as parents");
                    }
                case IssueType.TASK:
                    if (parent.type == IssueType.EPIC || parent.type == IssueType.FEATURE)
                    {
                        this.parent = parent;
                        this.parent.children.Add(this);
                        return;
                    } else
                    {
                        throw new InvalidOperationException("Tasks only have Features and Epics as parents");
                    }
            }
        }
    }
}
