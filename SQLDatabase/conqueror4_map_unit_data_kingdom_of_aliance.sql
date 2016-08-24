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
-- Table structure for table `map_unit_data_kingdom_of_aliance`
--

DROP TABLE IF EXISTS `map_unit_data_kingdom_of_aliance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `map_unit_data_kingdom_of_aliance` (
  `unitid` int(11) DEFAULT NULL,
  `xloc` int(11) DEFAULT NULL,
  `yloc` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map_unit_data_kingdom_of_aliance`
--

LOCK TABLES `map_unit_data_kingdom_of_aliance` WRITE;
/*!40000 ALTER TABLE `map_unit_data_kingdom_of_aliance` DISABLE KEYS */;
INSERT INTO `map_unit_data_kingdom_of_aliance` VALUES (7,150,2300),(7,500,2300),(7,600,2200),(7,350,2200),(7,250,2100),(7,500,2050),(7,150,1900),(7,350,1950),(7,500,1750),(7,300,1750),(7,150,1650),(7,400,1600),(7,300,1500),(7,500,1400),(7,150,1350),(7,250,1150),(7,350,1150),(8,300,1100),(9,250,2850),(10,350,350),(10,700,150),(10,750,500),(10,1100,300),(10,1350,100),(10,150,100),(11,2000,2900),(12,2100,2200),(12,1900,1900),(12,2250,1800),(12,2650,2100),(12,2150,1450),(12,2450,1550),(12,2750,1350),(12,2700,1850),(12,2450,2400),(12,3000,1650),(12,2950,2000),(12,2900,2350);
/*!40000 ALTER TABLE `map_unit_data_kingdom_of_aliance` ENABLE KEYS */;
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
