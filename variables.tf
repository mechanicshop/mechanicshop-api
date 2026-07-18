variable "db_connection_string" {
  type        = string
  description = "Connection string for Neon PostgreSQL"
  sensitive   = true
}

variable "jwt_secret" {
  type        = string
  description = "JWT Signing Secret Key"
  sensitive   = true
}

variable "grafana_loki_password" {
  type        = string
  description = "Grafana Loki API Key / Password"
  sensitive   = true
}

variable "grafana_loki_uri" {
  type        = string
  description = "The Grafana Loki ingest endpoint URI"
}

variable "grafana_loki_user" {
  type        = string
  description = "Grafana Loki user ID"
}

variable "subscription_id" {
  type        = string
  description = "Azure subscription ID"
  sensitive   = true
}

variable "github_token" {
  type        = string
  description = "GitHub Personal Access Token with repo and packages scope"
  sensitive   = true
}
