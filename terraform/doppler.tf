# Doppler data source to fetch secrets dynamically when use_doppler is enabled
data "doppler_secrets" "app" {
  count   = var.use_doppler ? 1 : 0
  project = var.doppler_project
  config  = var.doppler_config
}

locals {
  db_connection_string = var.use_doppler ? try(
    data.doppler_secrets.app[0].map.DB_CONNECTION_STRING,
    data.doppler_secrets.app[0].map.ConnectionStrings__DefaultConnection,
    data.doppler_secrets.app[0].map.CONNECTIONSTRINGS__DEFAULTCONNECTION,
    data.doppler_secrets.app[0].map.DATABASE_URL,
    var.db_connection_string
  ) : var.db_connection_string

  jwt_secret = var.use_doppler ? try(
    data.doppler_secrets.app[0].map.JWT_SECRET,
    data.doppler_secrets.app[0].map.JwtSettings__Secret,
    data.doppler_secrets.app[0].map.JWTSETTINGS__SECRET,
    var.jwt_secret
  ) : var.jwt_secret

  grafana_loki_uri = var.use_doppler ? try(
    data.doppler_secrets.app[0].map.GRAFANA_LOKI_URI,
    data.doppler_secrets.app[0].map.SERILOG__WRITETO__1__ARGS__URI,
    var.grafana_loki_uri
  ) : var.grafana_loki_uri

  grafana_loki_user = var.use_doppler ? try(
    data.doppler_secrets.app[0].map.GRAFANA_LOKI_USER,
    data.doppler_secrets.app[0].map.SERILOG__WRITETO__1__ARGS__CREDENTIALS__LOGIN,
    var.grafana_loki_user
  ) : var.grafana_loki_user

  grafana_loki_password = var.use_doppler ? try(
    data.doppler_secrets.app[0].map.GRAFANA_LOKI_PASSWORD,
    data.doppler_secrets.app[0].map.SERILOG__WRITETO__1__ARGS__CREDENTIALS__PASSWORD,
    var.grafana_loki_password
  ) : var.grafana_loki_password

  github_token = var.use_doppler ? try(
    data.doppler_secrets.app[0].map.GITHUB_TOKEN,
    data.doppler_secrets.app[0].map.GHCR_PULL_SECRET,
    var.github_token
  ) : var.github_token
}
