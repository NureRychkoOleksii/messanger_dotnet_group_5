namespace BLL
{
    public static class Status
    {
        public static readonly string UserNotLoggedIn = "User is not logged in.";
        
        public static readonly string AlreadyLoggedIn = "User is already logged in.";
        
        public static readonly string UserNameNotUnique = "Username is not unique";

        public static readonly string InvalidPassword = "Invalid password";

        public static readonly string PasswordsNotMatch = "Passwords do not match";

        public static readonly string InvalidEmail = "Invalid email";

        public static readonly string SuccessfullyInvitedUser = "Successfully invited user";

        public static readonly string NoSuchUserExists = "No such user exists";

        public static readonly string RoomAlreadyExists = "Such room already exists";

        public static readonly string RoleAlreadyExists = "Role already exists";

        public static readonly string SuccessfullyLoggedIn = "Successfully logged in!";

        public static readonly string SuccessfullyLoggedOut = "Successfully logged out.";

        public static readonly string InvalidLoginData =
            "Such user does not exist or the password is wrong, try again";

        public static readonly string SuccessfullyCreatedRoom = "Successfully created room!";

        public static readonly string RoomNameEmpty = "Room name can not be empty.";

        public static readonly string NotInTheRoom = "You are not in the room right now.";

        public static readonly string NoSuchRoomFound = "No such room found";

        public static readonly string NoRoomsFound = "You have no rooms";

        public static readonly string SuccessfullyEnteredRoom = "Successfully entered room!";

        public static readonly string InvitationsFound =
            "You are invited in room {0}, do you want to accept this invitation?";

        public static readonly string InvitationAccepted = "You have accepted the invitation to the room!";

        public static readonly string InvitationDeclined = "You have declined the invitation.";

        public static readonly string NoInvitationsFound = "You have zero invitations";

        public static readonly string NoChatsFound = "No chats in the current room found.";

        public static readonly string ChatAlreadyExists = "Such chat already exists";
        
        public static readonly string ChatNameEmpty = "Chat name can not be empty.";
        
        public static readonly string NotInTheChat = "You are not in the chat right now.";

        public static readonly string SuccessfullyEnteredChat = "Successfully entered chat!";
        
        public static readonly string SuccessfullyExitedChat = "Successfully exited chat!";

        public static readonly string NoSuchChatFound = "No such chat found";

        public static readonly string SuccessfullyExitedRoom = "Successfully exited room!";
        
        public static readonly string SuccessfullyCreatedChat = "Successfully created chat!";

        public static readonly string StatusOk = "Ok";

    }
}