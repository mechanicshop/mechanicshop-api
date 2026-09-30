output "resource_group_name" {
  description = "Name of the resource group"
  value       = azurerm_resource_group.app_rg.name
}

output "api_fqdn" {
  description = "FQDN of the API Container App"
  value       = azurerm_container_app.api.ingress[0].fqdn
}

output "client_fqdn" {
  description = "FQDN of the Client Container App"
  value       = azurerm_container_app.client.ingress[0].fqdn
}

output "azure_ad_client_id" {
  description = "Client ID / AppId of the Azure AD Application for GitHub Actions"
  value       = azuread_application.mechanicshop_app.client_id
}

output "azure_ad_service_principal_id" {
  description = "Object ID of the Service Principal"
  value       = azuread_service_principal.mechanicshop_sp.object_id
}
