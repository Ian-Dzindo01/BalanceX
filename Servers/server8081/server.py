import os
from flask import Flask, request, jsonify
import argparse
import logging
from waitress import serve
from azure.identity import DefaultAzureCredential
from azure.keyvault.secrets import SecretClient

app = Flask(__name__)

KEY_VAULT_URL = "https://balancex.vault.azure.net/"
SECRET_NAME = "X-API-KEY"

def get_api_key():
    credential = DefaultAzureCredential()
    client = SecretClient(vault_url=KEY_VAULT_URL, credential=credential)
    secret = client.get_secret(SECRET_NAME)
    return secret.value

API_KEY = get_api_key()

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(message)s')

@app.before_request
def log_request_info():
    client_ip = request.remote_addr
    headers = request.headers
    connection_header = headers.get("Connection", "Unknown")
    logging.info(f"Received request from {client_ip}, Connection Header: {connection_header}")

@app.before_request
def check_api_key():
    api_key = request.headers.get('X-API-KEY')
    if not API_KEY or api_key != API_KEY:
        logging.warning(f"Unauthorized access attempt from {request.remote_addr}")
        return jsonify({"error": "Unauthorized"}), 401

@app.route("/")
def index():
    return "Hello, this is the main page!"

@app.route("/health")
def health():
    return "OK", 200  

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--port", type=int, required=True, help="Port number")
    args = parser.parse_args()

    serve(app, host='0.0.0.0', port=args.port)
