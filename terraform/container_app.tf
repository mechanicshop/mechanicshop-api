resource "azurerm_container_app" "api" {
  name                         = "mechanic-shop-api"
  container_app_environment_id = data.azurerm_container_app_environment.existing.id
  resource_group_name          = azurerm_resource_group.app_rg.name
  revision_mode                = "Single"
  workload_profile_name        = "Consumption"

  secret {
    name  = "ghcr-pull-secret"
    value = local.github_token
  }

  registry {
    server               = "ghcr.io"
    username             = "moamenelbarky"
    password_secret_name = "ghcr-pull-secret"
  }

  template {
    min_replicas = 0
    max_replicas = 1

    container {
      name   = "mechanic-shop-api"
      image  = "ghcr.io/mechanicshop/mechanic-shop-api:latest"
      cpu    = "0.5"
      memory = "1Gi"

      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "Production"
      }
      env {
        name  = "ASPNETCORE_URLS"
        value = "http://+:8080"
      }
      env {
        name  = "ConnectionStrings__DefaultConnection"
        value = local.db_connection_string
      }
      env {
        name  = "JwtSettings__Secret"
        value = local.jwt_secret
      }
      env {
        name  = "JwtSettings__Issuer"
        value = "https://mechanic-shop-api.internal"
      }
      env {
        name  = "JwtSettings__Audiences__0"
        value = "https://mechanic-shop-client.azurecontainerapps.io"
      }
      env {
        name  = "AutoMigrateDb"
        value = "true"
      }
      env {
        name  = "SERILOG__USING__1"
        value = "Serilog.Sinks.Grafana.Loki"
      }
      env {
        name  = "SERILOG__WRITETO__1__NAME"
        value = "GrafanaLoki"
      }
      env {
        name  = "SERILOG__WRITETO__1__ARGS__URI"
        value = local.grafana_loki_uri
      }
      env {
        name  = "SERILOG__WRITETO__1__ARGS__LABELS__0__KEY"
        value = "app"
      }
      env {
        name  = "SERILOG__WRITETO__1__ARGS__LABELS__0__VALUE"
        value = "MechanicShop.Api"
      }
      env {
        name  = "SERILOG__WRITETO__1__ARGS__LABELS__1__KEY"
        value = "env"
      }
      env {
        name  = "SERILOG__WRITETO__1__ARGS__LABELS__1__VALUE"
        value = "production"
      }
      env {
        name  = "SERILOG__WRITETO__1__ARGS__CREDENTIALS__LOGIN"
        value = local.grafana_loki_user
      }
      env {
        name  = "SERILOG__WRITETO__1__ARGS__CREDENTIALS__PASSWORD"
        value = local.grafana_loki_password
      }
      env {
        name  = "SERILOG__WRITETO__1__ARGS__PERIOD"
        value = "00:00:01"
      }
      env {
        name  = "SERILOG__WRITETO__1__ARGS__EAGERLYEMITFIRSTEVENT"
        value = "true"
      }
    }
  }

  ingress {
    allow_insecure_connections = true
    external_enabled           = false
    target_port                = 8080
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  lifecycle {
    ignore_changes = [
      template[0].container[0].image,
      template[0].container[0].liveness_probe,
      template[0].container[0].readiness_probe,
      template[0].container[0].startup_probe,
      template[0].container[0].env,
      secret,
      registry
    ]
  }
}
