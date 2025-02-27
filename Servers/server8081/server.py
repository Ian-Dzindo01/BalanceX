from flask import Flask
import argparse

app = Flask(__name__)

@app.route("/")
def index():
    return "Hello, this is the main page!"

@app.route("/health")
def health():
    return "OK", 200  # Health check endpoint

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--port", type=int, required=True, help="Port number")
    args = parser.parse_args()

    app.run(host="0.0.0.0", port=args.port)
