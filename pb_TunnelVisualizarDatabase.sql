-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema pb_TunnelVisualizarDatabase
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema pb_TunnelVisualizarDatabase
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `pb_TunnelVisualizarDatabase` DEFAULT CHARACTER SET utf8 ;
USE `pb_TunnelVisualizarDatabase` ;

-- -----------------------------------------------------
-- Table `pb_TunnelVisualizarDatabase`.`sensor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pb_TunnelVisualizarDatabase`.`sensor` (
  `iddata` INT NULL AUTO_INCREMENT,
  `position_x` VARCHAR(45) NULL,
  `position_y` VARCHAR(45) NULL,
  `description` VARCHAR(45) NULL,
  PRIMARY KEY (`iddata`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `pb_TunnelVisualizarDatabase`.`data_waterLavel`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pb_TunnelVisualizarDatabase`.`data_waterLavel` (
  `iddata_waterLavel` INT NULL AUTO_INCREMENT,
  `data` VARCHAR(45) NULL,
  `sensor_iddata` INT NOT NULL,
  PRIMARY KEY (`iddata_waterLavel`, `sensor_iddata`),
  INDEX `fk_data_waterLavel_sensor1_idx` (`sensor_iddata` ASC),
  CONSTRAINT `fk_data_waterLavel_sensor1`
    FOREIGN KEY (`sensor_iddata`)
    REFERENCES `pb_TunnelVisualizarDatabase`.`sensor` (`iddata`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `pb_TunnelVisualizarDatabase`.`data_waterFlow`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pb_TunnelVisualizarDatabase`.`data_waterFlow` (
  `iddata_waterFlow` INT NULL AUTO_INCREMENT,
  `data` VARCHAR(45) NULL,
  `sensor_iddata` INT NOT NULL,
  PRIMARY KEY (`iddata_waterFlow`, `sensor_iddata`),
  INDEX `fk_data_waterFlow_sensor1_idx` (`sensor_iddata` ASC),
  CONSTRAINT `fk_data_waterFlow_sensor1`
    FOREIGN KEY (`sensor_iddata`)
    REFERENCES `pb_TunnelVisualizarDatabase`.`sensor` (`iddata`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `pb_TunnelVisualizarDatabase`.`rule`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pb_TunnelVisualizarDatabase`.`rule` (
  `idrule` INT NULL AUTO_INCREMENT,
  `rule` VARCHAR(45) NULL,
  `sensor_iddata` INT NOT NULL,
  PRIMARY KEY (`idrule`, `sensor_iddata`),
  INDEX `fk_rule_sensor_idx` (`sensor_iddata` ASC),
  CONSTRAINT `fk_rule_sensor`
    FOREIGN KEY (`sensor_iddata`)
    REFERENCES `pb_TunnelVisualizarDatabase`.`sensor` (`iddata`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
