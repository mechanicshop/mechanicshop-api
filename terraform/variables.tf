variable "azure_subscription_id" {
  type        = string
  description = "Azure Subscription ID"
  default     = "83ab56f5-88ee-436d-87a5-994d3185bf00"
}

variable "github_owner" {
  type        = string
  description = "GitHub Organization or Owner"
  default     = "mechanicshop"
}

variable "api_repository_name" {
  type        = string
  description = "Backend repository name"
  default     = "mechanicshop-api"
}

variable "web_repository_name" {
  type        = string
  description = "Frontend repository name"
  default     = "mechanicshop-web"
}

variable "git_branch" {
  type        = string
  description = "Target deployment branch for OIDC federated credentials"
  default     = "main"
}

variable "db_connection_string" {
  type        = string
  description = "Connection string for Neon PostgreSQL"
  sensitive   = true
  default     = null
}

variable "jwt_secret" {
  type        = string
  description = "JWT Signing Secret Key"
  sensitive   = true
  default     = null
}

variable "grafana_loki_password" {
  type        = string
  description = "Grafana Loki API Key / Password"
  sensitive   = true
  default     = null
}

variable "grafana_loki_uri" {
  type        = string
  description = "The Grafana Loki ingest endpoint URI"
  default     = null
}

variable "grafana_loki_user" {
  type        = string
  description = "Grafana Loki user ID"
  default     = null
}

variable "github_token" {
  type        = string
  description = "GitHub Personal Access Token for GHCR pull secret"
  sensitive   = true
  default     = null
}

variable "doppler_token" {
  type        = string
  description = "Optional Doppler Service Token (can also be provided via DOPPLER_TOKEN env var)"
  sensitive   = true
  default     = null
}

variable "doppler_project" {
  type        = string
  description = "Doppler Project Name"
  default     = "mechanicshop-api"
}

variable "doppler_config" {
  type        = string
  description = "Doppler Config / Environment Name"
  default     = "prd"
}
