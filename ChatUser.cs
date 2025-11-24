using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public enum UserStatusType
    {
        Offline, Away, Busy, Idle, Available
    }

    public enum RequestStatus
    {
        Unread, Read, Accepted, Rejected
    }

    public class UserManager
    {
        private static UserManager _instance;

        /* maps from a user id to a user */
        private Dictionary<int, ChatUser> usersById;

        /* maps from an account name to a user */
        private Dictionary<String, ChatUser> usersByAccountName;

        /* maps from the user id to an online user */
        private Dictionary<int, ChatUser> usersOnline;

        public static UserManager GetInstance()
        {
            if (_instance == null)
                _instance = new UserManager();

            return _instance;
        }

        public void AddUser(ChatUser fromUser, string toAccountName) { }
        public void signOnUser(string AccountName) { }
        public void signOffUser(string AccountName) { }
        public void approveAddRequest(AddRequest request) { }
        public void rejectAddRequest(AddRequest request) { }
        
    }

    /*
    The method receivedAddRequest, in the User class, notifies User B that User A has requested
    to add him. User B approves or rejects the request (via UserManager. approveAddRequest or
    rejectAddRequest), and the UserManager takes care of adding the users to each other's contact lists.
    The method sentAddRequest in the User class is called by UserManager to add an Add Request to
    User A's list of requests. So the flow is:
    1. User A clicks "add user" on the client and it gets sent to the server.
    2. User A calls requestAddUser(User B ).
    3. This method calls UserManager. addUser.
    4. UserManager calls both User A. sentAddRequest and
    User B.receivedAddRequest.
     */

    public class ChatUser
    {
        public int Id { get; private set; }
        public UserStatus Status { get; set; }
        public string AccountName { get; private set; }
        public string FullName { get; private set; }

        /* maps from the other participant's user id to the chat */
        private Dictionary<int, PrivateChat> privateChats;
        /* list of group chats */
        private List<GroupChat> groupChats;

        /* maps from the other person's user id to the add request */
        private Dictionary<int, AddRequest> receivedAddRequests;
        /* maps from the other person's user id to the add request */
        private Dictionary<int, AddRequest> sentAddRequests;

        /* maps from the user id to user object */
        private Dictionary<int, ChatUser> contacts;

        public ChatUser(int id, string accountName, string fullName)
        {
            Id = id;
            AccountName = accountName;
            FullName = fullName;
        }

        public bool SendMessageToUser(User to, string content) { return true; }
        public bool SendMessageToGroup(int id, string content) { return true; }
        public bool AddContact(User user) { return true; }
        public void receivedAddRequest(AddRequest req) {  }
        public void sentAddRequest(AddRequest req) {  }
    }

    public abstract class Converstation
    {
        protected List<ChatUser> Participants;
        public int Id { get; protected set; }
        public List<Message> Messages { get; protected set; }

        public bool AddMessage(Message message) {  return true; }
    }

    public class GroupChat : Converstation
    {
        public void AddParticipant(ChatUser user) { }
        public void RemoveParticipant(ChatUser user) { }
    }

    public class PrivateChat : Converstation
    {
        public PrivateChat(ChatUser user1, ChatUser user2){ }
        
        public ChatUser GetOtherParticipant(User primary) { return null; }
    }

    public class Message
    {
        public string Content { get; private set; }
        public DateOnly Date {  get; private set; }

        public Message(string content, DateOnly date)
        {
            Content = content;
            Date = date;
        }
    }

    public class AddRequest
    {
        public ChatUser FromUser { get; private set; }
        public ChatUser ToUser { get; private set; }
        public DateOnly Date { get; private set; }
        public RequestStatus Status { get; set; }

        public AddRequest(ChatUser userFrom, ChatUser userTo, DateOnly date)
        {
            FromUser = userFrom;
            ToUser = userTo;
            Date = date;
        }
    }

    public class UserStatus
    {
        public string Message { get; private set; }
        public UserStatusType Type { get; private set; }

        public UserStatus(string message, UserStatusType type)
        {
            Message = message;
            Type = type;
        }
    }
}
