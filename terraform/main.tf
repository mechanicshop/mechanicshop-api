resource "azurerm_resource_group" "app_rg" {
  name     = "mechanic-shop-rg"
  location = "swedencentral"
}

data "azurerm_container_app_environment" "existing" {
  name                = "quiznova-env"
  resource_group_name = "quiz-nova-resource-group"
}
