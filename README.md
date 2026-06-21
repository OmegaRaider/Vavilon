SQL часть


DROP TABLE IF EXISTS action_log CASCADE;
DROP TABLE IF EXISTS book_loans CASCADE;
DROP TABLE IF EXISTS book_copies CASCADE;
DROP TABLE IF EXISTS readers CASCADE;
DROP TABLE IF EXISTS librarians CASCADE;
DROP TABLE IF EXISTS departments CASCADE;
DROP TABLE IF EXISTS catalog_books CASCADE;

-- 1. Каталожная карточка (название книги)
CREATE TABLE catalog_books (
    book_id SERIAL PRIMARY KEY,
    udc_index VARCHAR(20),
    author VARCHAR(255) NOT NULL,
    title VARCHAR(255) NOT NULL,
    publisher VARCHAR(255),
    year_pub INT,
    pages INT,
    total_quantity INT DEFAULT 0
);

-- 2. Отделы библиотеки
CREATE TABLE departments (
    dept_id SERIAL PRIMARY KEY,
    dept_name VARCHAR(100) NOT NULL
);

-- 3. Читатели (абонементная карточка)
CREATE TABLE readers (
    reader_id SERIAL PRIMARY KEY,
    last_name VARCHAR(100) NOT NULL,
    first_name VARCHAR(100) NOT NULL,
    middle_name VARCHAR(100),
    address VARCHAR(255),
    phone VARCHAR(20),
    registration_date DATE DEFAULT CURRENT_DATE
);

-- 4. Экземпляры (формуляр)
CREATE TABLE book_copies (
    copy_id SERIAL PRIMARY KEY,
    book_id INT REFERENCES catalog_books(book_id) ON DELETE CASCADE,
    inventory_number VARCHAR(50) UNIQUE NOT NULL,
    dept_id INT REFERENCES departments(dept_id) ON DELETE SET NULL,
    condition VARCHAR(50) DEFAULT 'Хорошее'
);

-- 5. Библиотекари (пользователи системы)
CREATE TABLE librarians (
    lib_id SERIAL PRIMARY KEY,
    login VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    full_name VARCHAR(200),
    role VARCHAR(20) DEFAULT 'librarian'   -- 'admin' или 'librarian'
);

-- 6. Журнал выдачи (учёт выданных книг)
CREATE TABLE book_loans (
    loan_id SERIAL PRIMARY KEY,
    copy_id INT REFERENCES book_copies(copy_id) ON DELETE CASCADE,
    reader_id INT REFERENCES readers(reader_id) ON DELETE CASCADE,
    lib_id INT REFERENCES librarians(lib_id) ON DELETE SET NULL,
    issue_date DATE DEFAULT CURRENT_DATE,
    due_date DATE NOT NULL,
    return_date DATE
);

-- 7. Журнал действий (аудит)
CREATE TABLE action_log (
    log_id SERIAL PRIMARY KEY,
    user_login VARCHAR(50),
    action_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    action_type VARCHAR(50),
    table_affected VARCHAR(100),
    record_id INT,
    details TEXT
);


-- ТЕСТОВЫЕ ДАННЫЕ

INSERT INTO departments (dept_name) VALUES 
('Абонемент'), 
('Читальный зал');

INSERT INTO librarians (login, password_hash, full_name, role) VALUES 
('admin', 'admin123', 'Администратор Системы', 'admin'),
('lib1', 'lib123', 'Петрова Мария Ивановна', 'librarian');

INSERT INTO catalog_books (udc_index, author, title, publisher, year_pub, pages, total_quantity) VALUES 
('821.161.1', 'Пушкин А.С.', 'Капитанская дочка', 'Азбука', 2018, 384, 3),
('821.111', 'Толкин Дж.Р.Р.', 'Властелин колец', 'АСТ', 2020, 1450, 2),
('004.43', 'Рихтер Джеффри', 'CLR via C#', 'Питер', 2022, 896, 1);

INSERT INTO book_copies (book_id, inventory_number, dept_id) VALUES 
(1, 'Инв-001/1', 1), (1, 'Инв-001/2', 1), (1, 'Инв-001/3', 2),
(2, 'Инв-002/1', 1), (2, 'Инв-002/2', 2),
(3, 'Инв-003/1', 1);

INSERT INTO readers (last_name, first_name, middle_name, address, phone) VALUES 
('Иванов', 'Иван', 'Иванович', 'ул. Ленина, 5', '123-45-67'),
('Сидорова', 'Анна', 'Петровна', 'пр. Мира, 10', '89001112233');

INSERT INTO book_loans (copy_id, reader_id, lib_id, issue_date, due_date) VALUES 
(1, 1, 2, '2024-01-15', '2024-01-30');

-- ============================================================
-- ХРАНИМЫЕ ФУНКЦИИ (уровень доступа к данным)
-- ============================================================

-- Аутентификация
CREATE OR REPLACE FUNCTION fn_authenticate(
    p_login VARCHAR,
    p_password VARCHAR
) RETURNS VARCHAR AS $$
DECLARE
    v_role VARCHAR;
BEGIN
    SELECT role INTO v_role FROM librarians
    WHERE login = p_login AND password_hash = p_password;
    RETURN v_role;
END;
$$ LANGUAGE plpgsql;

-- Выдача книги
CREATE OR REPLACE FUNCTION fn_issue_book(
    p_librarian_login VARCHAR,
    p_inv_number VARCHAR,
    p_reader_id INT,
    p_days INT
) RETURNS INT AS $$
DECLARE
    v_copy_id INT;
    v_lib_id INT;
    v_loan_id INT;
BEGIN
    -- Найти свободный экземпляр
    SELECT copy_id INTO v_copy_id FROM book_copies
    WHERE inventory_number = p_inv_number
      AND copy_id NOT IN (
          SELECT copy_id FROM book_loans WHERE return_date IS NULL
      );
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Экземпляр не найден или уже выдан';
    END IF;

    -- Найти библиотекаря
    SELECT lib_id INTO v_lib_id FROM librarians WHERE login = p_librarian_login;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Библиотекарь не найден';
    END IF;

    -- Создать запись выдачи
    INSERT INTO book_loans (copy_id, reader_id, lib_id, issue_date, due_date)
    VALUES (v_copy_id, p_reader_id, v_lib_id, CURRENT_DATE, CURRENT_DATE + p_days)
    RETURNING loan_id INTO v_loan_id;

    -- Записать в журнал действий
    INSERT INTO action_log (user_login, action_type, table_affected, record_id, details)
    VALUES (p_librarian_login, 'INSERT', 'book_loans', v_loan_id,
            'Выдача: инв.' || p_inv_number || ' читателю ' || p_reader_id || ' на ' || p_days || ' дн.');

    RETURN v_loan_id;
END;
$$ LANGUAGE plpgsql;

-- Возврат книги
CREATE OR REPLACE FUNCTION fn_return_book(
    p_loan_id INT,
    p_librarian_login VARCHAR
) RETURNS BOOLEAN AS $$
BEGIN
    UPDATE book_loans 
    SET return_date = CURRENT_DATE 
    WHERE loan_id = p_loan_id AND return_date IS NULL;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Запись выдачи не найдена или книга уже возвращена';
    END IF;

    INSERT INTO action_log (user_login, action_type, table_affected, record_id, details)
    VALUES (p_librarian_login, 'UPDATE', 'book_loans', p_loan_id, 'Возврат книги');

    RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

-- Добавление читателя
CREATE OR REPLACE FUNCTION fn_add_reader(
    p_last_name VARCHAR,
    p_first_name VARCHAR,
    p_middle_name VARCHAR,
    p_address VARCHAR,
    p_phone VARCHAR,
    p_librarian_login VARCHAR
) RETURNS INT AS $$
DECLARE
    v_reader_id INT;
BEGIN
    INSERT INTO readers (last_name, first_name, middle_name, address, phone)
    VALUES (p_last_name, p_first_name, p_middle_name, p_address, p_phone)
    RETURNING reader_id INTO v_reader_id;

    INSERT INTO action_log (user_login, action_type, table_affected, record_id, details)
    VALUES (p_librarian_login, 'INSERT', 'readers', v_reader_id, 
            'Добавлен читатель: ' || p_last_name || ' ' || p_first_name);

    RETURN v_reader_id;
END;
$$ LANGUAGE plpgsql;

-- Поиск читателей с фильтрами (возвращает таблицу)
CREATE OR REPLACE FUNCTION fn_search_readers(
    p_fio VARCHAR DEFAULT NULL,
    p_phone VARCHAR DEFAULT NULL,
    p_address VARCHAR DEFAULT NULL,
    p_reg_from DATE DEFAULT NULL,
    p_reg_to DATE DEFAULT NULL
) RETURNS TABLE (
    reader_id INT,
    last_name VARCHAR,
    first_name VARCHAR,
    middle_name VARCHAR,
    address VARCHAR,
    phone VARCHAR,
    registration_date DATE
) AS $$
BEGIN
    RETURN QUERY
    SELECT r.reader_id, r.last_name, r.first_name, r.middle_name, r.address, r.phone, r.registration_date
    FROM readers r
    WHERE 
        (p_fio IS NULL OR (r.last_name || ' ' || r.first_name || ' ' || COALESCE(r.middle_name, '')) ILIKE '%' || p_fio || '%')
        AND (p_phone IS NULL OR r.phone ILIKE '%' || p_phone || '%')
        AND (p_address IS NULL OR r.address ILIKE '%' || p_address || '%')
        AND (p_reg_from IS NULL OR r.registration_date >= p_reg_from)
        AND (p_reg_to IS NULL OR r.registration_date <= p_reg_to)
    ORDER BY r.last_name;
END;
$$ LANGUAGE plpgsql;

-- Поиск книг с фильтрами
CREATE OR REPLACE FUNCTION fn_search_books(
    p_author VARCHAR DEFAULT NULL,
    p_title VARCHAR DEFAULT NULL,
    p_publisher VARCHAR DEFAULT NULL,
    p_year_from INT DEFAULT NULL,
    p_year_to INT DEFAULT NULL
) RETURNS TABLE (
    book_id INT,
    author VARCHAR,
    title VARCHAR,
    publisher VARCHAR,
    year_pub INT,
    total_quantity INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT c.book_id, c.author, c.title, c.publisher, c.year_pub, c.total_quantity
    FROM catalog_books c
    WHERE 
        (p_author IS NULL OR c.author ILIKE '%' || p_author || '%')
        AND (p_title IS NULL OR c.title ILIKE '%' || p_title || '%')
        AND (p_publisher IS NULL OR c.publisher ILIKE '%' || p_publisher || '%')
        AND (p_year_from IS NULL OR c.year_pub >= p_year_from)
        AND (p_year_to IS NULL OR c.year_pub <= p_year_to)
    ORDER BY c.author;
END;
$$ LANGUAGE plpgsql;

-- Получение списка всех выдач (для вкладки "Возврат")
CREATE OR REPLACE FUNCTION fn_get_loans()
RETURNS TABLE (
    loan_id INT,
    reader_name TEXT,
    book_info TEXT,
    inventory_number VARCHAR,
    issue_date DATE,
    due_date DATE,
    status TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        bl.loan_id,
        r.last_name || ' ' || r.first_name AS reader_name,
        cb.title || ' (' || cb.author || ')' AS book_info,
        bc.inventory_number,
        bl.issue_date,
        bl.due_date,
        CASE WHEN bl.return_date IS NULL THEN 'На руках' ELSE 'Возвращена' END AS status
    FROM book_loans bl
    JOIN book_copies bc ON bl.copy_id = bc.copy_id
    JOIN catalog_books cb ON bc.book_id = cb.book_id
    JOIN readers r ON bl.reader_id = r.reader_id
    ORDER BY bl.issue_date DESC;
END;
$$ LANGUAGE plpgsql;

-- Получение журнала действий
CREATE OR REPLACE FUNCTION fn_get_action_log()
RETURNS TABLE (
    log_id INT,
    user_login VARCHAR,
    action_time TIMESTAMP,
    action_type VARCHAR,
    table_affected VARCHAR,
    record_id INT,
    details TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM action_log
    ORDER BY action_time DESC
    LIMIT 200;
END;
$$ LANGUAGE plpgsql;

-- Вспомогательная функция: получить список всех читателей для комбобокса
CREATE OR REPLACE FUNCTION fn_get_readers_list()
RETURNS TABLE (reader_id INT, full_name TEXT) AS $$
BEGIN
    RETURN QUERY
    SELECT r.reader_id, r.last_name || ' ' || r.first_name AS full_name
    FROM readers r
    ORDER BY r.last_name;
END;
$$ LANGUAGE plpgsql;
