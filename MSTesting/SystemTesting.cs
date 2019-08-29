//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ISE172_project.Logic;
//using System.IO;
//using MileStoneClient.CommunicationLayer;

//namespace MSTesting
//{
//    [TestClass()]
//    public class SystemTesting
//    {
//        //------- Atrributes
//        private ChatRoom chatTesting;
//        private User userTesting;
//        //string urlTesting = "http://ise172.ise.bgu.ac.il";
//        private string url = "http://localhost";

//        [TestInitialize()]
//        public void setUp() //each test creating a new object and deleting files in order to test on new objects.
//        {
//            refreshStart();
//            this.chatTesting = new ChatRoom();
//        }

//        [TestMethod()]
//        public void chatRoomConstructorTest()
//        {
//            //file loading check
//            Assert.IsNotNull(chatTesting.Messages);
//            Assert.IsNotNull(chatTesting.Users);

//            //checking if after initialization, there is no user conncted
//            userTesting = chatTesting.LoggedInUser;
//            Assert.IsNull(userTesting);
//        }

//        [TestMethod()]
//        public void chatRoomRegistratoinTest()
//        {
            
//             // creating user and registering.
             
//            string nicknameTest = "ISE-Test";
//            string groupIDTest = "3";
//            chatTesting.registration(nicknameTest, groupIDTest);
//            userTesting = null;
//            userTesting = chatTesting.getUser(nicknameTest, groupIDTest);

//            //After registration the user supposed to exist in users list and therefor we expect it not to be null
//            Assert.IsNotNull(userTesting);
            
//            //trying to register again
//            Assert.ThrowsException<Exception>(() => chatTesting.registration(nicknameTest, groupIDTest));
            
//            //checking if it appears twice in data base
//            int counter = 0;
//            bool alreadyExits = false;
//            foreach (User usr in chatTesting.Users)
//            {
//                if (usr.NickName == nicknameTest && usr.Group_ID == groupIDTest)
//                {
//                    counter++;
//                }
//            }
//            if (counter > 1)
//            {
//                alreadyExits = true;
//            }
//            //after trying to register twice with the same user, checking for duplicates.
//            Assert.IsFalse(alreadyExits);
//        }

//        [TestMethod()]
//        public void chatRoomLoginTest()
//        {
//            string nicknameTest = "ISE-Test";
//            string groupIDTest = "3";
//            chatTesting.registration(nicknameTest, groupIDTest);
//            chatTesting.login(nicknameTest, groupIDTest);

//            //after loging in to the system, checking if the user has seccessfuly logged in.
//            Assert.IsNotNull(chatTesting.LoggedInUser);

//            //checking that the user is indeed the user that has looged in to the system
//            StringAssert.Equals(chatTesting.LoggedInUser.Group_ID, groupIDTest);
//            StringAssert.Equals(chatTesting.LoggedInUser.NickName, nicknameTest);

//            //creating a non existing user 
//            string nonExistingUser = "nonExistingUser";
//            string nonExistingGroupId = "12";
//            //after trying to login with non existing user, checking if the user is logged in
//            Assert.ThrowsException<Exception>(() => chatTesting.login(nonExistingUser, nonExistingGroupId));
//        }

//        [TestMethod()]
//        public void chatRoomSendMessageTest()
//        {
//            //Trsting send message without user
//            userTesting = null;
//            Assert.ThrowsException<Exception>(() => chatTesting.send("Testing with null user"));

//            chatTesting.registration("testingUser", "15");
//            chatTesting.login("testingUser", "15");
//            Message testMessage = chatTesting.LoggedInUser.sendMessage("Testing message sending");
//            IMessage testImsg = Communication.Instance.Send(url, "20", "isTheSameType", "isTheSameTypeMessage");
//            Message transferingToMessage = Message.convertImessage(testImsg);

//            //checking if the Imsg of the sent message was transfered into messgae object
//            Assert.ReferenceEquals(testMessage,testImsg);
//        }

//        [TestMethod()]
//        public void checkContentTest()
//        {
//            //checking if an invalid message is indeed considered invalid by the system
//            bool isValid = Message.checkContentValidity("more than 150 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
//            Assert.IsFalse(isValid);
//            isValid = Message.checkContentValidity("");
//            Assert.IsFalse(isValid);

//            //checking if a valid message is indeed valid
//            isValid = Message.checkContentValidity("less than 150");
//            Assert.IsTrue(isValid);
//        }

//        private void refreshStart()
//        {
//            if (File.Exists(@"localDB\userList.dat")) //deleting files
//            {
//                File.Delete(@"localDB\userList.dat");
//            }
//        }
//    }
//}
