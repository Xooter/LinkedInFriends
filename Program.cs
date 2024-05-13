using LinkedInFriends.Models;

LinkedIn linkedIn = new LinkedIn();

linkedIn.Login();

Thread.Sleep(500);

linkedIn.FollowUserInDialog();

