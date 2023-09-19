using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIRA
{
    internal class Assignment
    {
        public readonly Guid userID;
        public readonly Guid issueID;

        public Assignment(Guid userID, Guid issueID) {
            this.userID = userID;
            this.issueID = issueID;
        }
    }
}
