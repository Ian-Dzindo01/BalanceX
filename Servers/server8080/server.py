from flask import Flask, request
import argparse
import logging
from waitress import serve

app = Flask(__name__)

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(message)s')

@app.before_request
def log_request_info():
    client_ip = request.remote_addr
    headers = request.headers
    connection_header = headers.get("Connection", "Unknown")
    logging.info(f"Received request from {client_ip}, Connection Header: {connection_header}")

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
