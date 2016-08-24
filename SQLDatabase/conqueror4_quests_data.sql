-- MySQL dump 10.13  Distrib 5.7.12, for Win32 (AMD64)
--
-- Host: localhost    Database: conqueror4
-- ------------------------------------------------------
-- Server version	5.7.14-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `quests_data`
--

DROP TABLE IF EXISTS `quests_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quests_data` (
  `ID` int(11) DEFAULT NULL,
  `next_quest` int(11) DEFAULT NULL,
  `title` varchar(255) DEFAULT NULL,
  `description` text,
  `objectives` varchar(255) DEFAULT NULL,
  `quest_completioner_unit_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quests_data`
--

LOCK TABLES `quests_data` WRITE;
/*!40000 ALTER TABLE `quests_data` DISABLE KEYS */;
INSERT INTO `quests_data` VALUES (1,2,'Call to Arms!','War! Those demons on the other shore of Crystal River appeared just two days ago. They have started pillaging our villages and butchering out people. It is already too late for those on the other side of river but we can still at least save those who survived this initial purge. #NAME go and meet Lieutenant Landax up ahead. He will give you next instructions.','3:9',9),(2,3,'Wolfpack','Welcome #NAME to the front. Do you hear that howling? They are not just ordinary wolves.It\'s a pack of spawned beasts by those filthy demons occuping our lands. We need to stop them #NAME. They are trying to break in here. Yout first task is to make them stop.','1:7:10',9),(3,4,'Slay their leader','Good job out there young one. You managed to repel their attack. But they are still still out there waiting for their chance. We need to end this once and for all if we are to sleep safely. You ought to kill their leader. You will recognize him easily - it\'s the one with big white claws violet eyes and with significant textures on his body.','1:8:1',9),(4,5,'Kill them all!','They went berserk #NAME! They think they can break us by sending so many of their spawns. Go and take care of it. Kill as many as you can. Show them that quantity does not go before quality. Meanwhile I\'ll think of a way to stop them once and for all.','1:7:15',9),(5,6,'A plan of hope','Good news #NAME! Just a moment ago our scouts gave us the most valuable information to our case. These wolves or whatever they are are being spawned by enemy\'s warlocks which reside just north of here. Travel there and kill them #NAME. Hopefully it will put an end to this madness.','1:10:5',9),(6,7,'Pyresteel','You have done very well out there young one. You sure deserve a reward for your service to the Realm of Men. Take this token and bring it to Berloc Pyresteel. He is out smith in the front. Just follow this path to the east and it will lead you to him. Oh and please don\'t forget to send him my regards.','3:11',11),(7,8,'Pyrewood','You carry Lieutenant Landax\'s token I see. That\'s a worthy one lad. If you are here already you can len me a helping hand I suppose. Do you see that forest up ahead? I need wood from there to fuel my forges. The problem is that my workers refuse to go there since they saw some foul creatures there as they said. Can you please have a look in there? It\'s just north from here.','2:2600:1850:500',11),(8,9,'Forest of souls','So they were saying the truth. That is an unpleasant one. I cannot supply our troops in the battlefield if someone won\'t take care of this. Could you please clear the forest from those poor souls for me? I\'ll be much grteful to you.','1:12:15',11),(9,0,'Inform the lieutenant','They are very strange these souls my lad. I\'ve never seen something like that before. It sure is work of that demons. They appeared at the same time. I think it is best that you inform the lieutenant lad.','3:9',9),(0,0,'No quest','You have reached the end of the story line!','',-1);
/*!40000 ALTER TABLE `quests_data` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-08-23 14:13:14
