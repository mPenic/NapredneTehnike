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
  `Lozinka` varchar(300) DEFAULT NULL,
  `DatumRegistracije` datetime NOT NULL,
  `Status` varchar(50) NOT NULL,
  PRIMARY KEY (`KorisnikID`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=105 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Data for the table `korisnici` */

insert  into `korisnici`(`KorisnikID`,`Ime`,`Prezime`,`Email`,`Lozinka`,`DatumRegistracije`,`Status`) values 
(1,'Ivan','Ivić','ivan.ivic@email.com','hash_lozinke_1','2024-01-14 18:05:45','Aktivan'),
(2,'Ana','Anić','ana.anic@email.com','hash_lozinke_2','2024-01-14 18:05:45','Aktivan'),
(3,'Petar','Petrović','petar.petrovic@email.com','hash_lozinke_3','2024-01-14 18:05:45','Aktivan'),
(4,'mislav','penic','mislav.penic@tvz.hr','/VX/mYwgrw8PowaFTu1s1qRYU0g0rL1LZId4hrr4FdrCLPIQ','2024-02-08 06:48:51','Active'),
(5,'Pero','Perić','pero.peric@email.com','hgP+UZMSOvDpK7I7XyIuL5ThIK7BAduwL8Lu02BJ2nMCBCcA','2024-02-09 18:16:25','Active'),
(7,'Miki','Peki','miki.peki@email.com','je8dqZ6jk/ABQlczp2RYWOGtN3P00I8AZ0SAFF3UkRWEe805','2024-02-09 18:25:19','Active'),
(8,'Neko','Nesto','neko.nesto@email.com','f8Vvt53Y5TfT2QyuVmXoxFwmqVV2gjafB6l2QtB5S72E5OZO','2024-02-09 19:07:23','Active'),
(9,'Bitni','Registar','test.test@email.com','tC+ZX9PXtVRdbJORJfaBu468TmbrZUbX4FIv78PpYpCn7bTH','2024-02-09 19:21:02','Active'),
(10,'test2','test2','test.test2@email.com','W9UX9Muy7xl9jTTf340MRFTUhGYKQGSlU9PQC/0/cSN+zrI6','2024-02-12 21:30:19','Active'),
(13,'test3','test3','test.test3@email.com','Kj8AKcGv1nzhC0g8skEC9KIvX0lsGSAJjAx/VIwUYgW7RC2l','2024-02-12 21:42:20','Active'),
(14,'test4','test4','test4@email.com','U4zg3xkSrr1BFs6ItVwsKYLNAJ3lKnfcHjiEWqGpnOg4NIwN','2024-02-12 21:47:06','Active'),
(15,'test5','test5','test5@email.com','EHBQaufTRfTGGAv/3MsgxLdsB6Nbb7UMTe6fOI6OpAVnC8bt','2024-02-12 21:49:58','Active'),
(16,'test6','test6','test6@email.com','m6zHQGEb0UAgNKZUuDjOpCDabPGBSlffzhU/+upTSfxivH6U','2024-02-12 21:58:06','Active'),
(17,'test7','test7','test7@email.com','D1RffY5Chugqn6250wEriAtvQTN9xyvgUd8udP/iVY4LG/x1','2024-02-12 23:46:59','Active'),
(19,'test8','test8','test8@email.com','m+rTKAjQxbsVFab8ll4xlQGTrCAY3zY/Vd+1FMrQKlTcd47Q','2024-02-13 02:08:32','Active'),
(20,'test9','test9','test9@email.com','5cI/p/wQARoKEE3mXMpC6h+9/K1faRU1CpDx0YagEtbB7lSy','2024-02-13 02:21:31','Active'),
(21,'test10','test10','test10@email.com','OdjPtq1E79D8Cq+8RHuRtWMphD7YHuOi3vX0z+LnrkS8uFd4','2024-02-14 23:50:21','Active'),
(22,'test12','test12','test12@email.com','/xzKVMniKxKhPHVG2TZlfsondlnBD8xbZTBIazwZKI9VM3yB','2024-02-15 00:19:57','Active'),
(23,'test14','test14','test14@email.com','KyD58dyOBLq+2K6yYLsbvIPghO+JTf/2eHVkHcvjVEmpn1cd','2024-02-15 00:33:32','Active'),
(24,'test20','test20','test20@email.com','NpOG3n6ncYsLSw4Lu/BRjQnnnNcSQrj81+jbBzmLCMzKbFCu','2024-02-17 09:52:56','Active'),
(25,'test21','test21','test21@email.com','b2RRkLSCToG+mPsahop6RUh1RHItcVqAkQZm8MvGfe/Wcp0u','2024-02-17 11:14:24','Active'),
(26,'test22','test22','test22@email.com','nPfiyJadqtJmGh4Sfucqpt9BJnjeIsnQhr+am42b3GSLdf63','2024-02-17 11:16:33','Active'),
(27,'test23','test23','test23@email.com','9ZB4VPS9KV3hwGMYe22Bhr0ss8Lyr51PRD1o0EEmmlBaQX6Q','2024-02-17 11:38:43','Active'),
(29,'test25','test25','test25@email.com','/JPex6Sqbp9D4Ci2kPLDC2t7PSgUhf2D3/Yi2tPaD8xpGfkd','2024-02-17 11:51:30','Active'),
(30,'test24','test24','test24@email.com','Q26a6X38vszq0Oscn0IRBhORMt6IMxHpEzfnFVLkT6xi2QcE','2024-02-17 15:11:12','Active'),
(31,'test26','test26','test26@email.com','pYihMl9HaxQEuYVxJtZNuwEioCmJi/m1x3mfUkkxqSHHQi1m','2024-02-17 15:38:33','Active'),
(32,'test27','test27','test27@email.com','bT6sw+VbSrj8f/5ERU3ZiskTbw7UJ7FW5Ntt8ifaulO4kmBV','2024-02-17 15:48:32','Active'),
(33,'test28','test28','test28@email.com','NTK94NFldVxI82v8C8559qM2+3TnjKZxol2YY6mEHVzyDU7p','2024-02-17 16:35:04','Active'),
(34,'test29','test29','test29@email.com','8dsHXV5sIG7q3Qmh4SPrTNgTRzvy2fMmL9hAgVk/Mp5Qqm7y','2024-02-17 16:37:15','Active'),
(35,'test30','test30','test30@email.com','WhYhOrBIwpbKoSVZ+sx0cq30fBfkfo0WGzl7l3IbOqCAcf7A','2024-02-17 22:44:45','Active'),
(36,'test31','test31','test31@email.com','UqDupROvJh5EKrIn6pERKSajtoJUEaZ8kb8pZSgUoed/7dVk','2024-02-18 19:09:28','Active'),
(38,'test32','test32','test32@email.com','RAXLUuA/2Gl4Bd04t3Q1WvwFWIV9RW5A9YTT5+C/QzqhaS/1','2024-02-18 19:51:22','Active'),
(39,'test33','test33','test33@email.com','KbCKxSUMfe/xGyi1D2U1RR/a/cOs1IyXeowyv+e4MoX/1AYQ','2024-02-18 20:56:07','Active'),
(40,'test34','test34','test34@email.com','lKTFh8pbHMMr/eECBxcgI6pahnY2WNHEm9LMfG+v2qB+XqaW','2024-02-18 21:00:27','Active'),
(41,'test35','test35','test35@email.com','PwE/5m5YbTo3q8iB9/ovdAUSMhdcevzKlXFJ1bUiCINQinLX','2024-02-18 21:06:13','Active'),
(42,'test36','test36','test36@email.com','YrthAB+/QwcYHa8ydbWdeMeOQ2JsKtT0oia21uG6TkkjunBh','2024-02-18 22:07:38','Active'),
(43,'test37','test37','test37@email.com','S4kl89vV0VIUtW/D3iG1oUnlAicEUNQ7Ary6uTEyRsrvxhqO','2024-05-19 15:45:41','Active'),
(44,'test38','test38','test38@email.com','3Id12b5H7BMB/gar6V90INOlDyUfxQCYaLXLPGBLEgHHI7YM','2024-05-19 15:50:22','Active'),
(45,'test39','test39','test39@email.com','f/p33YvtaTo7HA6jxG4bqA71ulLZJzJfTLZDcTiXPc/iiv2D','2024-05-19 15:51:41','Active'),
(46,'test40','test40','test40@email.com','1waVfN/QrqqEz75vREHYaPJWwPuFXQOe9ZbQmZGYGBqkRqx6','2024-05-19 19:11:18','Active'),
(47,'test41','test41','test41@email.com','oa48yTTefjtOJXgAOEadzto2Hf7pW37YTDD6HGTa6mx8SF2s','2024-05-19 19:21:23','Active'),
(48,'test42','test42','test42@email.com','m+osr3llikCXYSYR0n15R3KxETGbhxSXvIvtAn85157NnXrk','2024-05-19 19:26:12','Active'),
(50,'test43','test43','test43@email.com','WmZKcswWBK9noPnnqep3V53fYxY2bvo5+eCfIoEMi0l/8u/2','2024-05-19 20:23:41','Active'),
(51,'test44','test44','test44@email.com','wWcvKiX8DUn2UpSJwXSJPA+Nszcgku9dpDJZVpYtMTB8vsg9','2024-05-19 21:47:24','Active'),
(52,'test45','test45','test45@email.com','eLQjrObGgIUca6i8g30UAItASu6ABrlbS2x/33modPey2/gY','2024-05-19 21:55:34','Active'),
(53,'test46','test46','test46@email.com','3lqW/tXmCBL6O2qHJjcfJkqH9cPgzyOs5pWn0CgMSB9vrLxX','2024-05-19 23:12:54','Active'),
(54,'test47','test47','test47@email.com','trk2CErYbJwabhb/9CU6SiUqPEUB142HaFXR77CsMaMxhUdn','2024-05-19 23:28:55','Active'),
(55,'test48','test48','test48@email.com','n95mMK4R4744gDQaSShLCf9gmTKthrYiwuVK8pJpvd+m5o04','2024-05-19 23:39:07','Active'),
(56,'test49','test49','test49@email.com','Amg3LCl45tSHmVT3GnrMvejmhSqrtqA7dUsy7LVCQnfJM2i6','2024-05-20 00:05:20','Active'),
(57,'test50','test50','test50@email.com','ymOBmQ3c0b9WLYzHacYhR78cVk+qIWW6pFox+5EuR+6/bClR','2024-06-10 19:22:06','Active'),
(58,'test51','test51','test51@email.com','YpYPLRiQQErrlEFc5I1tlESpDDyJTmhqRSbEILEqSLOgaaxT','2024-06-26 20:12:01','Active'),
(59,'test52','test52','test52@email.com','NGbvPr23Du7JtYqN5zwWMWLV9837NihiupJNacQ71xrMXkSX','2024-06-26 20:13:02','Active'),
(60,'test53','test53','test53@email.com','fSiPXSe0PnkZukSBq1cgqQii0YSdDSX0TOWhPlRD9qOPrUPY','2024-06-26 22:32:31','Active'),
(61,'test54','test54','test54@email.com','W6sJ94RaIlWiiJTxClgD+zdTKrwQU6yM4ML14YFUU2ksd+5o','2024-06-27 12:25:10','Active'),
(62,'test55','test55','test55@email.com','Y7J+Pv4VroxOAhjwi6HYUPcPDQu1HENB6jZyX7S9U+/vWQkM','2024-06-27 13:11:13','Active'),
(63,'test56','test56','test56@email.com','','2024-06-27 13:23:54','Active'),
(64,'test57','test57','test57@email.com','fevJ2P61ckOACg8+FM3EYscdQytNJSsS0J97QJVQjXOsvDCm','2024-06-27 16:56:06','Active'),
(65,'test58','test58','test58@email.com','VAwP8O30MeLjOP2wzvn+9sdqH7WaXTwmhP/U/YXA7V6AWyQA','2024-06-27 17:07:13','Active'),
(67,'test59','test59','test59@email.com','4BJFoatW5p1AoOIT8kp8ykTUK6lAOgEoHhiDTcRpx0XIY5lG','2024-06-27 17:14:26','Active'),
(68,'test60','test60','test60@email.com','PRNeOYFuiAvV+UxbkS4j0EaEis/s+b3uAExaNX0wlZFnFx/H','2024-06-27 17:30:36','Active'),
(69,'test61','test61','test61@email.com','$2a$11$gJSRBHsjb7L2vg8TTtiZX.LjqlS9B4/1vpPN4gsG/s9HXPXt1ywi.','2024-06-27 17:42:05','Active'),
(70,'test62','test62','test62@email.com','$2a$11$kp8353Iz51xRG2R02OQj1Oipub5bjq5ToB8rcAg7P4jzSAaxyXMsu','2024-06-27 17:49:36','Active'),
(71,'test63','test63','test63@email.com','$2a$11$bUDLpfV4Jz8.E9HCymeAK.WGP9jmt0QR0a.g76zHjo59VPTtG.WYa','2024-06-27 17:54:14','Active'),
(72,'test64','test64','test64@email.com','$2a$11$8Zwgb7xErZ3YEDidtagfKufF2Efj3u.k0xo5YBB.dFvA3XHhURi3S','2024-06-27 18:08:17','Active'),
(73,'test65','test65','test65@email.com','$2a$11$UBoqklTWCk3XtZ6mXMXxIO9FBlSclrAh26NB99e3xnq3FrIm2TZU.','2024-06-27 18:14:36','Active'),
(74,'test66','test66','test66@email.com','$2a$11$N5/FhajPn4h0ljfiT/0N4O7b5DGQ1TX8nsc8YfYM.okokPdOIN7jy','2024-06-27 18:47:36','Active'),
(75,'test67','test67','test67@email.com','$2a$11$Fwrmd46wmm6.eaBjHOBKvOtq5DwA58XFAf6bRZU.QTrftlY7W76l2','2024-06-27 18:56:25','Active'),
(76,'test68','test68','test68@email.com','$2a$11$Pf6ZkxlQCbvdZjKPWg99WeA7RWznaTiw3eir/suPk0ITJrtk7xhNC','2024-06-27 19:03:23','Active'),
(77,'test69','test69','test69@email.com','$2a$11$38JRQjGsjywkMrtsFSUuBu0bc6UK6SfnTiCgGsvc2A4N0vxvbf72u','2024-06-27 19:08:00','Active'),
(78,'test70','test70','test70@email.com','$2a$11$22tUHqiGRmJjBIo.R2OpEeMPeMIGCsChL21Fhp62iGEuZ8bCYSkL2','2024-06-27 19:14:30','Active'),
(79,'test71','test71','test71@email.com','$2a$11$Z9hrcYtxw2/wSKfPeQPAPe8aVIPF0kOdj5WIExy4sCKa1C70AcvFG','2024-06-27 19:22:19','Active'),
(80,'test72','test72','test72@email.com','$2a$11$3AQrKo9Hj9oWoR6yIUXRYetbN7KjCiAbbEn7fHi1Cm1qftXX3Z.6S','2024-06-27 19:33:44','Active'),
(81,'test73','test73','test73@email.com','$2a$11$Oz9cx74UnFF.DbZB3aIjoOQ9s/Dp8FpOc967XEn/SONflBZ4qbTVS','2024-06-27 19:54:00','Active'),
(82,'test74','test74','test74@email.com','$2a$11$64CyNDohYt019Fgnk93NVuAMhVFT44QB8wyjxR7/sxmA3sw6NOjae','2024-06-27 20:58:00','Active'),
(83,'test75','test75','test75@email.com','$2a$11$WYpZhPC7zAyml0kC1FWaoOrVTW1DZzZrsCi7Pup.KenPULqy.7/W.','2024-06-27 20:58:33','Active'),
(84,'test76','test76','test76@email.com','$2a$11$85b2GPZd6DOMlG7yqVnyvemTu0TglxjySOj9/R45SDYhTQP549.Qu','2024-06-27 20:58:56','Active'),
(85,'test77','test77','test77@email.com','$2a$11$vnI1nPQL/iV3gw7cs.Yhs.inGRoN28tF6FE18UGOA94tefwIe7/kK','2024-06-27 21:09:32','Active'),
(86,'test78','test78','test78@email.com','$2a$11$l6Ex0su/0dK5jm0DxPXsbuzoMPq4Tw6MSHaSKRuQTIyt8.RgMbG1q','2024-06-27 21:16:25','Active'),
(87,'test79','test79','test79@email.com','$2a$11$Syge1k.AjlKy2Tjiv3GtDeIdvB4sW8kP55s1ZX/dGDy5QuXkRw9E6','2024-06-27 21:22:23','Active'),
(88,'test80','test80','test80@email.com','$2a$11$OtyCyYyHF.vdIprX6jGIruyN3.PZJrrPSZWaFJGjASmwm6onlPuS2','2024-06-27 21:31:55','Active'),
(89,'test81','test81','test81@email.com','$2a$11$E9v/F7vKkprh/lOU85WCJuBHpNV3QLxBVv6awXSGi4qLhwLF7c9Fy','2024-07-01 21:41:49','Active'),
(90,'test82','test82','test82@email.com','$2a$11$n/QPdXY0MGZ/pnSFSIxfp.ZMUdik97EOUemzK2FcuZfzKp2HVcqYW','2024-07-01 21:47:20','Active'),
(91,'test83','test83','test83@email.com','$2a$11$65X9Y5jJgpqAwBM/z9D7fOV7L0MIouSpkMPTsom8Q9dt3xg2w9Uz.','2024-07-01 21:48:59','Active'),
(93,'test84','test84','test84@email.com','$2a$11$Wz6970Oklbr7yX7K7S9dAeubo5m2l5DTEDEZR0.gR/ydy47btac5W','2024-07-01 21:50:33','Active'),
(94,'test85','test85','test85@email.com','$2a$11$Z52LrjKL7vPKm3b2EmiZveAl5TVB8Q3cs6gGFIwXMqG7JCLlnGmHO','2024-07-01 21:50:57','Active'),
(96,'test86','test86','test86@email.com','$2a$11$5VRvnTBMU..OGHX/p35j3u0q8cC/ZYUaDO98IR2mWBf8s7nG.315S','2024-07-02 11:20:15','Active'),
(97,'test87','test87','test87@email.com','$2a$11$Hsl27DQ6w3CBcpAlkHGQ0OownmGb7MGACyFp5rnU/h105HDjbjrW.','2024-07-02 11:46:18','Active'),
(99,'test88','test88','test88@email.com','$2a$11$KuHVOXDaEzj5XfVLRWbrcOEyXdNnYPj6QFtn4ELI2Dr9Z3PRfynQK','2024-07-02 11:53:39','Active'),
(100,'test89','test89','test89@email.com','$2a$11$KCDXbhCAJ1BOyNuOVpWF6OWL6yfb7CoXQ/AKcKuZtxrY2yMuCclTG','2024-07-02 12:02:16','Active'),
(101,'test90','test90','test90@email.com','$2a$11$a.w6.CNNGoL.r20PsT5nAuntmTHJsdsclgRe.Gfh1GGYTIjvPobni','2024-07-02 12:42:13','Active'),
(103,'test91','test91','test91@email.com','$2a$11$95t6eAdelelXY6l/5lb7tuSO3jPv.oUV99ZFZZyN8xl6h5DLO8q0G','2024-07-02 12:44:25','Active'),
(104,'test92','test92','test92@email.com','$2a$11$VBHhHFPwiT/ODFVOkKbqXO2UF9UKvkE58NJR38elB7IeBoeHS.6Xu','2024-07-02 21:02:45','Active');

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
  KEY `KorisnikID` (`KorisnikID`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Data for the table `racuni` */

insert  into `racuni`(`RacunID`,`KorisnikID`,`BrojRacuna`,`Stanje`,`DatumOtvaranja`,`Vrsta`,`Valuta`) values 
(1,1,'HR12345678901',10000.00,'2021-01-01 10:00:00','Tekući','HRK'),
(2,1,'HR12345678902',5000.00,'2021-02-01 10:00:00','Štednja','EUR'),
(3,2,'HR12345678903',7500.00,'2021-03-01 10:00:00','Tekući','HRK'),
(12,88,'NewAccount',-685.00,'2024-07-01 01:45:38','Savings','HRK'),
(14,88,'HR823724516',0.00,'2024-07-01 02:08:52','Savings','HRK'),
(15,88,'HR900385488',400.00,'2024-07-01 02:08:59','Savings','HRK'),
(16,88,'HR483283661',0.00,'2024-07-01 02:11:32','Savings','HRK'),
(17,88,'HR320738592',0.00,'2024-07-01 20:51:44','Savings','HRK'),
(20,100,'HR301902200',0.00,'2024-07-02 12:09:18','Savings','HRK'),
(21,100,'HR600327188',0.00,'2024-07-02 12:09:21','Savings','HRK'),
(22,0,'HR476108543',0.00,'2024-07-02 12:45:30','Savings','HRK'),
(23,101,'HR410596672',0.00,'2024-07-02 15:12:03','Savings','HRK'),
(24,101,'HR569947421',1140.00,'2024-07-02 16:40:14','Savings','HRK'),
(25,0,'HR168122848',0.00,'2024-07-02 21:02:51','Savings','HRK'),
(26,104,'HR273452872',0.00,'2024-07-02 21:03:15','Savings','HRK'),
(27,104,'HR221552481',0.00,'2024-07-02 21:03:23','Savings','HRK'),
(28,94,'HR975919726',0.00,'2024-07-02 22:05:09','Savings','HRK'),
(29,101,'HR482923600',0.00,'2024-07-02 22:40:23','Savings','HRK'),
(30,101,'HR645198067',0.00,'2024-07-03 03:00:09','Savings','EUR');

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
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Data for the table `transakcije` */

insert  into `transakcije`(`TransakcijaID`,`RacunID`,`DatumVrijeme`,`Iznos`,`Vrsta`,`Opis`) values 
(6,15,'2024-07-01 02:10:49',100.00,'Transfer','New transaction'),
(7,15,'2024-07-01 02:10:51',100.00,'Transfer','New transaction'),
(8,15,'2024-07-01 02:10:54',100.00,'Transfer','New transaction'),
(9,12,'2024-07-01 02:11:40',100.00,'Transfer','New transaction'),
(10,12,'2024-07-01 02:11:47',100.00,'Transfer','New transaction'),
(11,12,'2024-07-01 02:13:37',100.00,'Transfer','New transaction'),
(12,15,'2024-07-01 02:35:31',69.00,'Credit','User transaction'),
(13,12,'2024-07-01 02:35:47',-1000.00,'Debit','User transaction'),
(14,15,'2024-07-01 20:34:20',-69.00,'Debit','User transaction'),
(16,24,'2024-07-02 19:32:41',1000.00,'Credit','User transaction'),
(17,24,'2024-07-02 19:32:47',-10.00,'Debit','User transaction'),
(18,24,'2024-07-02 19:32:54',75.00,'Credit','User transaction'),
(19,24,'2024-07-02 19:32:57',75.00,'Credit','User transaction');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
