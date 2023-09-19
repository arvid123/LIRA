using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LIRA
{
    public class Board
    {
        Dictionary<Guid, User> users = new Dictionary<Guid, User>();
        Dictionary<Guid, Issue> issues = new Dictionary<Guid, Issue>();
        List<Assignment> assignments = new List<Assignment>();

        public Guid AddIssue(string title, IssueType type)
        {
            Guid id = Guid.NewGuid();
            issues[id] = new Issue(title, type);
            return id;
        }

        /**
        <summary>
        Remove issue with id issueID completely from the board forever.
        When the issue is removed, its children will get its parent as their parent. If the removed issue
        had no parent, its children will no longer have a parent.
        </summary>
        <param name="issueID">The ID of the issue to be removed</param>
        **/
        public void RemoveIssue(Guid issueID)
        {
            issues.Remove(issueID);
        }

        /**
        <summary>
        If the new state is Done, the operation fails if the issue has children or grand-children that are
        not Done.
        </summary>
        <param name="issueID">ID of the issue whose state is being set</param>
        <param name="state">The new state the issue is being set to</param>
        **/
        public void SetIssueState(Guid issueID, IssueState state)
        {
            issues[issueID].SetState(state);
        }

        /**
        <summary>
        The following rules apply:
        - An Epic can have no parent.
        - A Story can have an Epic as a parent.
        - A Task can have an Epic or a Story as parent.
        - If any rule is broken, the operation fails and nothing happens.
        </summary>
        <param name="issueID">ID of the issue whose parent is being set</param>
        <param name="parentIssueID">The parent's ID.  If null, the issue will no longer have a
        parent.
        </param>
        **/
        public void SetParentIssue(Guid issueID, Guid parentIssueID)
        {
            issues[issueID].SetParent(issues[parentIssueID]);
        }

        /**
        <param name="userID">ID of the user to assign. If null, the issue no
        longer has a user assigned to it.
        </param>
        <param name="issueID">ID of the issue to assign the user to.</param>
        **/
        public void AssignUser(Guid? userID, Guid issueID)
        {
            if (assignments.Where(x => x.userID == userID && x.issueID == issueID).Count() > 0) return;

            if (userID == null)
            {
                int assignmentsCountBefore = assignments.Count();
                assignments = assignments.Where(x => x.issueID != issueID).ToList();
                int assignmentsCountAfter = assignments.Count();
                
                if(assignmentsCountBefore == assignmentsCountAfter)
                {
                    throw new Exception("Trying to remove assignments from issue which doesn't have any assignments");
                }

                return;
            }

            assignments.Add(new Assignment((Guid)userID, issueID));
        }

        /**
        <summary>
        Retrieve a collection of issues, optionally filtered by a set of parameters. If all filter parameters
        are null, all issues will be returned.
        </summary>
        <param name="state">Filter by state that the issue is in.
        </param>
        <param name="userID">Filter by which issues a specific user is assigned to</param>
        <param name="issueTypes">A list of issue types by which to filter</param>
        <param name="startDate">The beginning of the time interval to filter by (inclusive), default value is the beginning of time</param>
        <param name="endDate">The end of the time interval to filter by (exclusive), default value is the end of time</param>
        **/
        public List<Issue> GetIssues(IssueState? state = null, Guid? userID = null, List<IssueType>? issueTypes = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate > endDate) throw new Exception("Invalid Time Interval for Issue filtering");
            return issues
                .Where(x => state == null || x.Value.state == state)
                .Where(x => userID == null || isAssigned((Guid)userID, x.Key))
                .Where(x => issueTypes == null || issueTypes.Contains(x.Value.type))
                .Where(x => startDate == null || x.Value.creationTime >= startDate)
                .Where(x => endDate == null || x.Value.creationTime < endDate)
                .Select(x => x.Value)
                .ToList();
        }

        private bool isAssigned(Guid userID, Guid issueID) 
        {
            return assignments.Where(x => x.issueID == issueID && x.userID == userID).Count() > 0;
        }

        /**
        <param name="issueID">ID of the issue to return.</param>
        **/
        public Issue GetIssue(Guid issueID) 
        {
            return issues[issueID];
        }

        /**
        <summary>
        Creates a new user and returns the unique ID identifying the user.
        </summary>
        <param name="name">Name of the new user.</param>
        **/
        public Guid AddUser(string name) {
            Guid id = Guid.NewGuid();
            users[id] = new User(name);
            return id;
        }

        /**
        <summary>
        Remove a user completely and unassign from any issues this user is currently assigned to.
        </summary>
        <param name="userID">ID of the user to be removed.</param>
        **/
        public void RemoveUser(Guid userID) {
            users.Remove(userID);
            assignments = assignments.Where(x => x.userID != userID).ToList();
        }

        /**
        <summary>
        Returns a list of all users that are currently in the system.
        </summary>
        **/
        public List<User> GetUsers() {
            return users.Values.ToList();
        }

        public User GetUser(Guid userID)
        {
            return users[userID];
        }
    }
}
