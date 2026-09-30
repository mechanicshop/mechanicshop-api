# Doppler data source to fetch secrets dynamically
data "doppler_secrets" "app" {
  project = var.doppler_project
  config  = var.doppler_config
}

locals {
  db_connection_string  = try(data.doppler_secrets.app.map.DB_CONNECTION_STRING, var.db_connection_string)
  jwt_secret            = try(data.doppler_secrets.app.map.JWT_SECRET, var.jwt_secret)
  grafana_loki_uri      = try(data.doppler_secrets.app.map.GRAFANA_LOKI_URI, var.grafana_loki_uri)
  grafana_loki_user     = try(data.doppler_secrets.app.map.GRAFANA_LOKI_USER, var.grafana_loki_user)
  grafana_loki_password = try(data.doppler_secrets.app.map.GRAFANA_LOKI_PASSWORD, var.grafana_loki_password)
  github_token          = try(data.doppler_secrets.app.map.GITHUB_TOKEN, var.github_token)
}
