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
-- Table structure for table `characters_data`
--

DROP TABLE IF EXISTS `characters_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `characters_data` (
  `spec` int(11) DEFAULT NULL,
  `name` varchar(255) DEFAULT NULL,
  `hitpoints` int(11) DEFAULT NULL,
  `manapoints` int(11) DEFAULT NULL,
  `damage` int(11) DEFAULT NULL,
  `armor` int(11) DEFAULT NULL,
  `spellbonus` int(11) DEFAULT NULL,
  `level` int(11) DEFAULT NULL,
  `fraction` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `characters_data`
--

LOCK TABLES `characters_data` WRITE;
/*!40000 ALTER TABLE `characters_data` DISABLE KEYS */;
INSERT INTO `characters_data` VALUES (1,'Demon Hunter',120,75,18,3,0,1,1),(1,'Demon Hunter',140,85,20,4,0,2,1),(1,'Demon Hunter',165,100,24,6,0,3,1),(2,'Mage',90,90,23,1,0,1,1),(2,'Mage',100,100,28,1,0,2,1),(2,'Mage',115,115,34,3,0,3,1),(3,'Priest',80,95,15,0,0,1,1),(3,'Priest',90,115,17,1,0,2,1),(3,'Priest',105,135,19,1,0,3,1),(4,'Warlock',70,95,25,0,0,1,2),(4,'Warlock',80,105,31,0,0,2,2),(4,'Warlock',90,120,39,1,0,3,2),(6,'Shaman',85,90,15,1,0,1,2),(6,'Shaman',100,105,17,2,0,2,2),(6,'Shaman',115,130,19,3,0,3,2);
/*!40000 ALTER TABLE `characters_data` ENABLE KEYS */;
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
