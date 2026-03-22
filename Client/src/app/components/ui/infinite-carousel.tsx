import { motion } from "framer-motion";
import { Card, CardContent } from "@/app/components/ui/card";
import { Button } from "@/app/components/ui/button";

const products = [
  {
    id: 1,
    title: "iPhone 15 Pro",
    price: 999,
    image: "https://images.unsplash.com/photo-1695048133142-1a20484d2569"
  },
  {
    id: 2,
    title: "Gaming Laptop",
    price: 1499,
    image: "https://images.unsplash.com/photo-1603302576837-37561b2e2302"
  },
  {
    id: 3,
    title: "Headphones",
    price: 199,
    image: "https://images.unsplash.com/photo-1518441902110-82a5b9c6b86c"
  },
  {
    id: 4,
    title: "Smart Watch",
    price: 299,
    image: "https://images.unsplash.com/photo-1516574187841-cb9cc2ca948b"
  }
];

// 🔥 Дублируем для бесконечности
const loopProducts = [...products, ...products];

export const InfiniteCarousel = () => {
  return (
    <div className="overflow-hidden w-full">

      <motion.div
        className="flex gap-6"
        animate={{ x: ["0%", "-50%"] }}
        transition={{
          ease: "linear",
          duration: 20,
          repeat: Infinity
        }}
      >
        {loopProducts.map((p, i) => (
          <div key={i} className="min-w-[250px]">
            <Card className="bg-[#151821] border-gray-800 hover:scale-105 transition">
              <CardContent className="p-4">
                <img
                  src={p.image}
                  className="h-40 w-full object-cover rounded-xl mb-4"
                />

                <h3 className="font-semibold">{p.title}</h3>
                <p className="text-green-400 font-bold mb-2">
                  ${p.price}
                </p>

                <Button size="sm" className="w-full">
                  Buy
                </Button>
              </CardContent>
            </Card>
          </div>
        ))}
      </motion.div>

    </div>
  );
};