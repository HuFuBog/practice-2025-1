import os
import json
from pathlib import Path

CONFIG_FILE = "config.json" # Имя конфигурационного файла (создастся рядом со скриптом)
OUTPUT_FILENAME = "project_files_dump.txt" # имя выходного файла

def create_default_config():
    """Создает конфигурационный файл по умолчанию, если его нет."""
    default_config = {
        "include_extensions": [".py", ".json"],
        "exclude_files": ["__init__.py", OUTPUT_FILENAME],
        "exclude_dirs": [".git", "venv", "env", "__pycache__", ".idea", ".vscode", "node_modules"]
    }
    
    if not os.path.exists(CONFIG_FILE):
        with open(CONFIG_FILE, 'w', encoding='utf-8') as f:
            json.dump(default_config, f, indent=4, ensure_ascii=False)
        print(f"[*] Создан файл конфигурации по умолчанию: {CONFIG_FILE}")
    
    return default_config

def load_config():
    """Загружает настройки из JSON файла."""
    if not os.path.exists(CONFIG_FILE):
        print(f"Код собирается в соответствии с правилами из {CONFIG_FILE}")
        return create_default_config()
        
    with open(CONFIG_FILE, 'r', encoding='utf-8') as f:
        return json.load(f)

def collect_files(root_path_str):
    root_path = Path(root_path_str).resolve()
    
    if not root_path.exists() or not root_path.is_dir():
        print(f"[!] Ошибка: Путь '{root_path}' не существует или не является папкой.")
        return

    config = load_config()
    output_path = root_path / OUTPUT_FILENAME

    print(f"[*] Анализ директории: {root_path}")
    print(f"[*] Ищем файлы: {', '.join(config['include_extensions'])}")
    print(f"[*] Исключаем файлы: {', '.join(config['exclude_files'])}")

    processed_count = 0

    with open(output_path, 'w', encoding='utf-8') as out_file:
        for file_path in root_path.rglob('*'):
            if not file_path.is_file():
                continue

            # Проверка на исключенные папки (чтобы не лезть в venv, .git и т.д.)
            if any(exclude_dir in file_path.parts for exclude_dir in config['exclude_dirs']):
                continue

            # Проверка на исключенные файлы
            if file_path.name in config['exclude_files']:
                continue

            # Проверка на нужное расширение
            if file_path.suffix not in config['include_extensions']:
                continue

            # Получаем относительный путь (от корня проекта) и меняем слеши на unix-style для красоты
            rel_path = file_path.relative_to(root_path).as_posix()
            
            try:
                content = file_path.read_text(encoding='utf-8')
                
                # Записываем в нужном формате
                out_file.write(f"[/{rel_path}]\n")
                out_file.write(f"{content}\n\n")
                
                processed_count += 1
            except UnicodeDecodeError:
                print(f"[!] Пропущен файл (не текстовый формат или ошибка кодировки): {rel_path}")
            except Exception as e:
                print(f"[!] Ошибка при чтении {rel_path}: {e}")

    print(f"[+] Готово! Обработано файлов: {processed_count}")
    print(f"[+] Результат сохранен в: {output_path}")

if __name__ == "__main__":
    print("=== Сборщик кода проекта ===")
    # target_dir = input("Введите полный путь к корневой папке проекта: ").strip()
    target_dir = "/home/kirieshka/practice-2025-1/subproject/Roguelike/Assets/".strip()
    # Убираем кавычки, если путь был скопирован через "Copy as path" в Windows или что то подобное
    if target_dir.startswith('"') and target_dir.endswith('"'):
        target_dir = target_dir[1:-1]
        
    collect_files(target_dir)
