/*
SQLyog Community v13.2.0 (64 bit)
MySQL - 10.4.27-MariaDB : Database - banka
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`banka` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;

USE `banka`;

/*Table structure for table `korisnici` */

DROP TABLE IF EXISTS `korisnici`;

CREATE TABLE `korisnici` (
  `KorisnikID` int(11) NOT NULL AUTO_INCREMENT,
  `Ime` varchar(100) NOT NULL,
  `Prezime` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Lozinka` varchar(255) NOT NULL,
  `DatumRegistracije` datetime NOT NULL,
  `Status` varchar(50) NOT NULL,
  PRIMARY KEY (`KorisnikID`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Data for the table `korisnici` */

insert  into `korisnici`(`KorisnikID`,`Ime`,`Prezime`,`Email`,`Lozinka`,`DatumRegistracije`,`Status`) values 
(1,'Ivan','Ivić','ivan.ivic@email.com','hash_lozinke_1','2024-01-14 18:05:45','Aktivan'),
(2,'Ana','Anić','ana.anic@email.com','hash_lozinke_2','2024-01-14 18:05:45','Aktivan'),
(3,'Petar','Petrović','petar.petrovic@email.com','hash_lozinke_3','2024-01-14 18:05:45','Aktivan');

/*Table structure for table `logovi` */

DROP TABLE IF EXISTS `logovi`;

CREATE TABLE `logovi` (
  `LogID` int(11) NOT NULL AUTO_INCREMENT,
  `KorisnikID` int(11) DEFAULT NULL,
  `DatumVrijeme` datetime NOT NULL,
  `Opis` varchar(255) NOT NULL,
  PRIMARY KEY (`LogID`),
  KEY `KorisnikID` (`KorisnikID`),
  CONSTRAINT `logovi_ibfk_1` FOREIGN KEY (`KorisnikID`) REFERENCES `korisnici` (`KorisnikID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Data for the table `logovi` */

insert  into `logovi`(`LogID`,`KorisnikID`,`DatumVrijeme`,`Opis`) values 
(1,1,'2021-04-04 15:00:00','Prijavio se u sustav'),
(2,2,'2021-04-05 16:00:00','Promijenio lozinku'),
(3,NULL,'2021-04-06 17:00:00','Sustav ažuriran');

/*Table structure for table `racuni` */

DROP TABLE IF EXISTS `racuni`;

CREATE TABLE `racuni` (
  `RacunID` int(11) NOT NULL AUTO_INCREMENT,
  `KorisnikID` int(11) NOT NULL,
  `BrojRacuna` varchar(50) NOT NULL,
  `Stanje` decimal(15,2) NOT NULL,
  `DatumOtvaranja` datetime NOT NULL,
  `Vrsta` varchar(50) NOT NULL,
  `Valuta` varchar(10) NOT NULL,
  PRIMARY KEY (`RacunID`),
  UNIQUE KEY `BrojRacuna` (`BrojRacuna`),
  KEY `KorisnikID` (`KorisnikID`),
  CONSTRAINT `racuni_ibfk_1` FOREIGN KEY (`KorisnikID`) REFERENCES `korisnici` (`KorisnikID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Data for the table `racuni` */

insert  into `racuni`(`RacunID`,`KorisnikID`,`BrojRacuna`,`Stanje`,`DatumOtvaranja`,`Vrsta`,`Valuta`) values 
(1,1,'HR12345678901',10000.00,'2021-01-01 10:00:00','Tekući','HRK'),
(2,1,'HR12345678902',5000.00,'2021-02-01 10:00:00','Štednja','EUR'),
(3,2,'HR12345678903',7500.00,'2021-03-01 10:00:00','Tekući','HRK');

/*Table structure for table `transakcije` */

DROP TABLE IF EXISTS `transakcije`;

CREATE TABLE `transakcije` (
  `TransakcijaID` int(11) NOT NULL AUTO_INCREMENT,
  `RacunID` int(11) NOT NULL,
  `DatumVrijeme` datetime NOT NULL,
  `Iznos` decimal(15,2) NOT NULL,
  `Vrsta` varchar(50) NOT NULL,
  `Opis` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`TransakcijaID`),
  KEY `RacunID` (`RacunID`),
  CONSTRAINT `transakcije_ibfk_1` FOREIGN KEY (`RacunID`) REFERENCES `racuni` (`RacunID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Data for the table `transakcije` */

insert  into `transakcije`(`TransakcijaID`,`RacunID`,`DatumVrijeme`,`Iznos`,`Vrsta`,`Opis`) values 
(1,1,'2021-04-01 12:00:00',2000.00,'Uplata','Plaća'),
(2,2,'2021-04-02 13:00:00',150.00,'Isplata','Kupovina'),
(3,1,'2021-04-03 14:00:00',500.00,'Isplata','Računi');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
