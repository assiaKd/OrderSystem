terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.67"
    }
  }

  required_version = ">= 1.3"
}

provider "azurerm" {
  features {}
}

variable "resource_group_name" {
  default = "order-system-2-rg"
}

variable "location" {
  default = "westus2"
}

variable "order_service_tag" {
  default = "latest"
}

variable "inventory_service_tag" {
  default = "latest"
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

locals {
  rabbit_user     = "guest"
  rabbit_password = "guest"
  rabbit_host     = azurerm_container_app.rabbitmq.name
}

resource "azurerm_redis_cache" "order_redis" {
  name                        = "order-redis-2"
  location                    = azurerm_resource_group.rg.location
  resource_group_name         = azurerm_resource_group.rg.name
  capacity                    = 1
  family                      = "C"
  sku_name                    = "Standard"
  minimum_tls_version          = "1.2"
  public_network_access_enabled = true
}

resource "azurerm_container_app_environment" "env" {
  name                = "order-system-2-env"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_container_app" "order_service" {
  name                         = "order-service-2"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

     ingress {
      external_enabled = true
      target_port      = 8080
      transport        = "auto"
	  
	  traffic_weight {
      percentage      = 100
      latest_revision = true
    }
    }
  template {
    container {
      name   = "order-2"
      image  = "assia008/order-service:${var.order_service_tag}"
      cpu    = 0.5
      memory = "1Gi"

      env {
        name  = "EventBusSettings__HostAddress"
        value = "amqp://${local.rabbit_user}:${local.rabbit_password}@${local.rabbit_host}:5672?heartbeat=60"
      }

      env {
        name  = "RABBITMQ_HOST"
        value = "rabbitmq"
      }
    }
  }
}

resource "azurerm_container_app" "inventory_service" {
  name                         = "inventory-service-2"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

     ingress {
      external_enabled = true
      target_port      = 8080
      transport        = "auto"
	  
	  traffic_weight {
      percentage      = 100
      latest_revision = true
    }
    }
  template {
    container {
      name   = "inventory"
      image  = "assia008/inventory-service:${var.inventory_service_tag}"
      cpu    = 0.5
      memory = "1Gi"

      env {
        name  = "EventBusSettings__HostAddress"
        value = "amqp://${local.rabbit_user}:${local.rabbit_password}@${local.rabbit_host}:5672?heartbeat=60"
      }

      env {
        name  = "RABBITMQ_HOST"
        value = "rabbitmq"
      }
    }
  }
}

resource "azurerm_container_app" "rabbitmq" {
  name                         = "rabbitmq2"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

  template {
    container {
      name   = "rabbitmq2"
      image  = "rabbitmq:3-management"
      cpu    = 0.5
      memory = "1Gi"

      env {
        name  = "RABBITMQ_DEFAULT_USER"
        value = "guest"
      }

      env {
        name  = "RABBITMQ_DEFAULT_PASS"
        value = "guest"
      }
    }
  }
}