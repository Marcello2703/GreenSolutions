import { type FormEvent, useEffect, useState } from "react";

type CrudApi<TEntity extends { id: number }, TForm> = {
  list: () => Promise<TEntity[]>;
  create: (payload: TForm) => Promise<TEntity>;
  update: (id: number, payload: TForm) => Promise<TEntity>;
  remove: (id: number) => Promise<void>;
  toForm: (entity: TEntity) => TForm;
};

export function useCrudSection<TEntity extends { id: number }, TForm>(
  api: CrudApi<TEntity, TForm>,
  emptyForm: TForm
) {
  const [items, setItems] = useState<TEntity[]>([]);
  const [form, setForm] = useState<TForm>(emptyForm);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);

  async function refresh() {
    setLoading(true);
    try {
      setItems(await api.list());
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    let cancelled = false;

    async function initialLoad() {
      try {
        const data = await api.list();
        if (!cancelled) {
          setItems(data);
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    }

    void initialLoad();

    return () => {
      cancelled = true;
    };
  }, [api]);

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (editingId === null) {
      await api.create(form);
    } else {
      await api.update(editingId, form);
    }

    setForm(emptyForm);
    setEditingId(null);
    await refresh();
  }

  function startEdit(item: TEntity) {
    setEditingId(item.id);
    setForm(api.toForm(item));
  }

  function cancelEdit() {
    setEditingId(null);
    setForm(emptyForm);
  }

  async function remove(id: number) {
    await api.remove(id);

    if (editingId === id) {
      cancelEdit();
    }

    await refresh();
  }

  return {
    items,
    form,
    setForm,
    editingId,
    loading,
    submit,
    startEdit,
    cancelEdit,
    remove,
    refresh
  };
}