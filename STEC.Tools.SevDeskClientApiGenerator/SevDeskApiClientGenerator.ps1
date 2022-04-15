#
# Script to generate SevDesk Client APIs
#


# Install
# Github Repository https://github.com/OpenAPITools/openapi-generator
Invoke-WebRequest -OutFile openapi-generator-cli.jar https://repo1.maven.org/maven2/org/openapitools/openapi-generator-cli/5.0.0/openapi-generator-cli-5.0.0.jar
java -jar openapi-generator-cli.jar help 
 
# API Dokumentation https://hilfe.sevdesk.de/knowledge/sevdesk-rest-full-api 

# Use parameter --skip-validate-spec to disable validation

# Generate Contact API
java -jar openapi-generator-cli.jar generate -i https://6083.extern.sevdesk.dev/OpenAPI/ContactAPI/openApi.json -g csharp-netcore -o SevDeskContactApiClient

# Generate Voucher API 
java -jar openapi-generator-cli.jar generate -i https://6083.extern.sevdesk.dev/OpenAPI/VoucherAPI/openApi.json -g csharp-netcore -o SevDeskVoucherApiClient

# Generate Invoice API 
java -jar openapi-generator-cli.jar generate -i https://6083.extern.sevdesk.dev/OpenAPI/InvoiceAPI/openApi.json -g csharp-netcore -o SevDeskInvoiceApiClient

# Generate Order API 	
java -jar openapi-generator-cli.jar generate -i https://6083.extern.sevdesk.dev/OpenAPI/OrderAPI/openApi.json -g csharp-netcore -o SevDeskOrderApiClient

# Generate Inventory API
java -jar openapi-generator-cli.jar generate -i https://6083.extern.sevdesk.dev/OpenAPI/InventoryAPI/openApi.json -g csharp-netcore -o SevDeskInventoryApiClient