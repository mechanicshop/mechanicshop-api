resource "azuread_application" "mechanicshop_app" {
  display_name = "github-actions-mechanic-shop"
}

resource "azuread_service_principal" "mechanicshop_sp" {
  client_id = azuread_application.mechanicshop_app.client_id
}

resource "azurerm_role_assignment" "mechanicshop_contributor" {
  scope                = azurerm_resource_group.app_rg.id
  role_definition_name = "Contributor"
  principal_id         = azuread_service_principal.mechanicshop_sp.object_id
}

# --- Federated Credentials for API Repository ---
resource "azuread_application_federated_identity_credential" "api_main" {
  application_id = azuread_application.mechanicshop_app.id
  display_name   = "github-actions-mechanicshop-api-main"
  description    = "Federated credential for mechanicshop-api main branch"
  audiences      = ["api://AzureADTokenExchange"]
  issuer         = "https://token.actions.githubusercontent.com"
  subject        = "repo:${var.github_owner}/${var.api_repository_name}:ref:refs/heads/${var.git_branch}"
}

resource "azuread_application_federated_identity_credential" "api_main_enhanced" {
  application_id = azuread_application.mechanicshop_app.id
  display_name   = "github-actions-mechanicshop-api-enhanced"
  description    = "Federated credential for mechanicshop-api enhanced format"
  audiences      = ["api://AzureADTokenExchange"]
  issuer         = "https://token.actions.githubusercontent.com"
  subject        = "repo:${var.github_owner}@335052983/${var.api_repository_name}@1282598834:ref:refs/heads/${var.git_branch}"
}
