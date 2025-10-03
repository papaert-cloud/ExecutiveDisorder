from flask import Flask, render_template, send_from_directory, redirect
import os

app = Flask(__name__)

# WebGL build directory
WEBGL_DIR = os.path.join(os.path.dirname(__file__), 'webgl')

# Disable caching for development
@app.after_request
def add_header(response):
    response.headers['Cache-Control'] = 'no-store, no-cache, must-revalidate, post-check=0, pre-check=0, max-age=0'
    response.headers['Pragma'] = 'no-cache'
    response.headers['Expires'] = '-1'
    return response

@app.route('/')
def index():
    webgl_exists = os.path.exists(os.path.join(WEBGL_DIR, 'index.html'))
    if webgl_exists:
        return redirect('/game')
    return render_template('index.html')

@app.route('/docs')
def docs():
    return render_template('index.html')

@app.route('/game')
def game():
    webgl_index = os.path.join(WEBGL_DIR, 'index.html')
    if os.path.exists(webgl_index):
        return send_from_directory(WEBGL_DIR, 'index.html')
    return render_template('no_build.html'), 404

@app.route('/game/<path:filename>')
def serve_webgl(filename):
    return send_from_directory(WEBGL_DIR, filename)

@app.route('/health')
def health():
    return {'status': 'healthy'}, 200

if __name__ == '__main__':
    port = int(os.environ.get('PORT', 5000))
    app.run(host='0.0.0.0', port=port, debug=True)
