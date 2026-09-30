# ==============================================================================
# Declarative Imports for Existing Azure AD, Azure RBAC, and GitHub Resources
# ==============================================================================

# 1. Existing Resource Group
import {
  to = azurerm_resource_group.app_rg
  id = "/subscriptions/83ab56f5-88ee-436d-87a5-994d3185bf00/resourceGroups/mechanic-shop-rg"
}

# 2. Existing Container App - API
import {
  to = azurerm_container_app.api
  id = "/subscriptions/83ab56f5-88ee-436d-87a5-994d3185bf00/resourceGroups/mechanic-shop-rg/providers/Microsoft.App/containerApps/mechanic-shop-api"
}

# 3. Existing Container App - Client
import {
  to = azurerm_container_app.client
  id = "/subscriptions/83ab56f5-88ee-436d-87a5-994d3185bf00/resourceGroups/mechanic-shop-rg/providers/Microsoft.App/containerApps/mechanic-shop-client"
}

# 4. Existing Azure AD Application
import {
  to = azuread_application.mechanicshop_app
  id = "/applications/b090c041-6479-4477-8403-b7054f127667"
}

# 5. Existing Service Principal
import {
  to = azuread_service_principal.mechanicshop_sp
  id = "c849bac5-50c6-4192-b788-bd3de4ac0318"
}

# 6. Existing Role Assignment (Contributor on mechanic-shop-rg)
import {
  to = azurerm_role_assignment.mechanicshop_contributor
  id = "/subscriptions/83ab56f5-88ee-436d-87a5-994d3185bf00/resourceGroups/mechanic-shop-rg/providers/Microsoft.Authorization/roleAssignments/9d847dec-6de4-43ad-98bc-a9b3f07ae98c"
}

# 7. Federated Identity Credentials
import {
  to = azuread_application_federated_identity_credential.api_main
  id = "b090c041-6479-4477-8403-b7054f127667/federatedIdentityCredential/a9302589-c339-4a66-bc27-83af0732ebe5"
}

import {
  to = azuread_application_federated_identity_credential.api_main_enhanced
  id = "b090c041-6479-4477-8403-b7054f127667/federatedIdentityCredential/b1f16d32-06fd-4141-b9de-2300f4bf461c"
}

import {
  to = azuread_application_federated_identity_credential.web_main
  id = "b090c041-6479-4477-8403-b7054f127667/federatedIdentityCredential/6d8988f3-cccb-427f-976a-dc47e34bf061"
}

import {
  to = azuread_application_federated_identity_credential.web_main_enhanced
  id = "b090c041-6479-4477-8403-b7054f127667/federatedIdentityCredential/0858365e-df37-42f3-bbe4-14b394c4a0c3"
}

# 8. GitHub Actions Secrets - API
import {
  to = github_actions_secret.api_azure_client_id
  id = "mechanicshop-api:AZURE_CLIENT_ID"
}

import {
  to = github_actions_secret.api_azure_tenant_id
  id = "mechanicshop-api:AZURE_TENANT_ID"
}

import {
  to = github_actions_secret.api_azure_subscription_id
  id = "mechanicshop-api:AZURE_SUBSCRIPTION_ID"
}

# 9. GitHub Actions Secrets - Web
import {
  to = github_actions_secret.web_azure_client_id
  id = "mechanicshop-web:AZURE_CLIENT_ID"
}

import {
  to = github_actions_secret.web_azure_tenant_id
  id = "mechanicshop-web:AZURE_TENANT_ID"
}

import {
  to = github_actions_secret.web_azure_subscription_id
  id = "mechanicshop-web:AZURE_SUBSCRIPTION_ID"
}
