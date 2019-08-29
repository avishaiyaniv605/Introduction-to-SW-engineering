//using System;
//using System.IO;
//using ISE172_project.Logic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace MSTesting
//{
//    [TestClass]
//    public class FileHandlerTesting
//    {

//        //------- Atrributes
//        private ChatRoom chatTesting;

//        [TestInitialize()]
//        public void setUp() //each test creating a new object and deleting files in order to test on new objects.
//        {
//            refreshStart();
//            this.chatTesting = new ChatRoom();
//        }

//        [TestMethod]
//        public void creatNewSystemFilesTesting()
//        {
//            //registering in order to create new users file.
//            chatTesting.registration("newTestingUser","2");

//            //asserting a users file was indeed created
//            Assert.IsTrue(File.Exists(@"localDB\userList.dat"));

//            //loging in and sending a message in order to create a new messages file.
//            chatTesting.login("newTestingUser", "2");
//            chatTesting.send("message testing");
//            chatTesting.retrieveMessages(10);
//            //cheking if after sending a messgae , a new messgae file is creter
//            Assert.IsTrue(File.Exists(@"localDB\msgList.dat"));
//        }

//        private void refreshStart() // deleting files
//        {
//            if (File.Exists(@"localDB\userList.dat"))
//            {
//                File.Delete(@"localDB\userList.dat");
//            }
//            if (File.Exists(@"localDB\msgList.dat"))
//            {
//                File.Delete(@"localDB\msgList.dat");
//            }
//        }
//    }
//}
